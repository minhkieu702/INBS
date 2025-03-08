using AutoMapper;
using INBS.Application.Common;
using INBS.Application.DTOs.Service.DesignService;
using INBS.Application.DTOs.Service.Service;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Common;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class ServiceService(IMapper _mapper, IUnitOfWork _unitOfWork, IFirebaseService _firebaseService) : IServiceService
    {
        private async Task InsertCategoryService(
            Guid serviceId, 
            IList<int> categoryIds, 
            IEnumerable<CategoryService> existedCategoryServices)
        {
            var existingCategoryIds = existedCategoryServices.Select(cs => cs.CategoryId).ToHashSet();

            var list = new List<CategoryService>();

            var categories = await Utils.GetCategories();

            foreach (var categoryId in categoryIds.Distinct())
            {
                if (existingCategoryIds.Contains(categoryId)) continue;

                try
                {
                    var category = categories.FirstOrDefault(c => c.ID == categoryId);
                    if (category != null)
                    {
                        var categoryService = new CategoryService
                        {
                            CategoryId = categoryId,
                            ServiceId = serviceId
                        };
                        list.Add(categoryService);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to process CategoryService with CategoryId: {categoryId}, Error: {ex.Message}");
                }
            }

            await _unitOfWork.CategoryServiceRepository.InsertRangeAsync(list);
        }

        private async Task UpdateDesignService(Guid serviceId, IList<ServiceDesignRequest> designIds)
        {
            if (designIds == null || !designIds.Any())
                return;

            await ValidateService(designIds.Select(c => c.DesignId));

            var oldDesignServices = await _unitOfWork.DesignServiceRepository.GetAsync(c => c.ServiceId == serviceId);
            
            if (oldDesignServices.Any())
                _unitOfWork.DesignServiceRepository.DeleteRange(oldDesignServices);

            var newDesignServices = new List<Domain.Entities.DesignService>();
            
            foreach (var designId in designIds)
            {
                newDesignServices.Add(new Domain.Entities.DesignService
                {
                    ServiceId = serviceId,
                    DesignId = designId.DesignId,
                    ExtraPrice = designId.ExtraPrice,
                });
            }
            await _unitOfWork.DesignServiceRepository.InsertRangeAsync(newDesignServices);
        }

        private async Task ValidateService(IEnumerable<Guid> designIds)
        {
            var designs = await _unitOfWork.DesignRepository.GetAsync(include: query => query.Where(c => !c.IsDeleted && designIds.Contains(c.ID)));
            if (designs.Count() != designIds.Count())
            {
                throw new Exception("Some design is not existed");
            }
        }

        public async Task Create(ServiceRequest modelRequest)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedEntity = await _unitOfWork.ServiceRepository.GetAsync(x => x.Name == modelRequest.Name);

                if (existedEntity != null && existedEntity.Any())
                    throw new Exception("Service already exists");

                var newService = _mapper.Map<Service>(modelRequest);

                newService.ImageUrl = modelRequest.NewImage != null ? await _firebaseService.UploadFileAsync(modelRequest.NewImage) : Constants.DEFAULT_IMAGE_URL;

                newService.CreatedAt = DateTime.Now;

                await _unitOfWork.ServiceRepository.InsertAsync(newService);

                await UpdateDesignService(newService.ID, modelRequest.Designs);

                var categoryServices = await _unitOfWork.CategoryServiceRepository.GetAsync(cs => cs.ServiceId.Equals(newService.ID));

                await InsertCategoryService(newService.ID, modelRequest.CategoryIds, categoryServices);

                if (await _unitOfWork.SaveAsync() == 0)
                    throw new Exception("Create service failed");
               
                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        private void DeleteCategoryService(IEnumerable<Domain.Entities.CategoryService> categoryServices)
        {
            _unitOfWork.CategoryServiceRepository.DeleteRange(categoryServices);
        }

        public async Task DeleteById(Guid id)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var isExist = await _unitOfWork.ServiceRepository.GetByIdAsync(id) ?? throw new Exception("Service not found");

                isExist.IsDeleted = true;

                await _unitOfWork.ServiceRepository.UpdateAsync(isExist);

                if (await _unitOfWork.SaveAsync() == 0)
                    throw new Exception("Delete service failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task<IEnumerable<ServiceResponse>> Get()
        {
            try
            {
                var services = await _unitOfWork.ServiceRepository.GetAsync(include: s => s.Where(service => !service.IsDeleted)
                  .Include(service => service.CategoryServices)
                  .Include(service => service.ServiceCustomCombos
                    .Where(scc => !scc.CustomCombo!.IsDeleted && !scc.CustomCombo.Customer!.User!.IsDeleted))
                    .ThenInclude(scc => scc.CustomCombo!)
                    .ThenInclude(customCombo => customCombo.Customer!)
                    .ThenInclude(customer => customer.User!)
                  .Include(service => service.DesignServices.Where(c => !c.Design!.IsDeleted))
                    .ThenInclude(service => service.Design)
                ) ?? throw new Exception("Something went wrong!");

                var responses = _mapper.Map<IEnumerable<ServiceResponse>>(services);

                if (responses.Any())
                {
                    var categories = await Utils.GetCategories();
                    foreach (var service in responses)
                    {
                        foreach (var cateService in service.CategoryServices)
                        {
                            cateService.Category = categories.FirstOrDefault(c => c.ID == cateService.CategoryId);
                        }
                    }
                }

                return responses;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task HandleCategoryServiceUpdating(Guid serviceId, IList<int> categoryIds)
        {
            var existedCategoryServices = await _unitOfWork.CategoryServiceRepository.GetAsync(cs => cs.ServiceId.Equals(serviceId));

            DeleteCategoryService(existedCategoryServices.Where(cs => !categoryIds.Contains(cs.CategoryId)));

            await InsertCategoryService(serviceId, categoryIds, existedCategoryServices);
        }

        public async Task Update(Guid id, ServiceRequest updatingRequest)
        {
            try
            {
                var existedEntity = await _unitOfWork.ServiceRepository.GetByIdAsync(id) ?? throw new Exception("Service not found");

                var existedEntityName = await _unitOfWork.ServiceRepository.GetAsync(filter: x => x.Name.Equals(updatingRequest.Name));

                if (existedEntityName != null && existedEntityName.Any() && existedEntityName.First().ID != id)
                    throw new Exception("Service already exists");

                _unitOfWork.BeginTransaction();

                var newEntity = _mapper.Map(updatingRequest, existedEntity);

                if (updatingRequest.NewImage != null)
                {
                    newEntity.ImageUrl = await _firebaseService.UploadFileAsync(updatingRequest.NewImage);
                }

                if (updatingRequest.CategoryIds != null && updatingRequest.CategoryIds.Any())
                    await HandleCategoryServiceUpdating(id, updatingRequest.CategoryIds);

                if (updatingRequest.Designs != null && updatingRequest.Designs.Any())
                    await UpdateDesignService(id, updatingRequest.Designs);

                await _unitOfWork.ServiceRepository.UpdateAsync(newEntity);

                if (await _unitOfWork.SaveAsync() == 0)
                    throw new Exception("Update service failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }
    }
}
