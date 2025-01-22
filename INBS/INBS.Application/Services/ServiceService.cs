using AutoMapper;
using INBS.Application.DTOs.Service;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class ServiceService(IMapper _mapper, IUnitOfWork _unitOfWork, IFirebaseService _firebaseService) : IServiceService
    {
        private async Task InsertCategoryService(Guid serviceId, IList<Guid> categoryIds, IEnumerable<Domain.Entities.CategoryService> existedCategoryServices)
        {
            // Lọc ra các CategoryId cần thêm mới
            var existingCategoryIds = existedCategoryServices.Select(cs => cs.CategoryId).ToHashSet(); // Dùng HashSet để tăng hiệu suất lookup
            var insertTasks = categoryIds
                .Distinct() // Loại bỏ ID trùng lặp
                .Where(id => !existingCategoryIds.Contains(id))
                .Select(async id =>
                {
                    try
                    {
                        var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                        if (category != null)
                        {
                            var categoryService = new Domain.Entities.CategoryService
                            {
                                CategoryId = id,
                                ServiceId = serviceId
                            };
                            await _unitOfWork.CategoryServiceRepository.InsertAsync(categoryService);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to delete CategoryService with ID: category-{id}, service-{serviceId}. Error: {ex.Message}");
                    }
                });

            await Task.WhenAll(insertTasks);

            if (insertTasks.Any(t => t.IsFaulted))
            {
                throw new Exception("Some category services could not be inserted successfully.");
            }
        }

        public async Task Create(ServiceCreatingRequest modelRequest)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedEntity = await _unitOfWork.ServiceRepository.GetAsync(x => x.Name == modelRequest.Name);

                if (existedEntity != null && existedEntity.Any())
                    throw new Exception("Service already exists");

                var newService = _mapper.Map<Service>(modelRequest);

                 newService.ImageUrl = modelRequest.Image != null ? 
                    await _firebaseService.UploadFileAsync(modelRequest.Image) :
                    "https://firebasestorage.googleapis.com/v0/b/fir-realtime-database-49344.appspot.com/o/images%2Fnoimage.jpg?alt=media&token=8ffe560a-6aeb-4a34-8ebc-16693bb10a56";

                newService.CreatedAt = DateTime.Now;

                //Insert service
                await _unitOfWork.ServiceRepository.InsertAsync(newService);

                var categoryServices = await _unitOfWork.CategoryServiceRepository.GetAsync(cs => cs.ServiceId.Equals(newService.ID));

                //Insert categoryServices
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

        private async Task DeleteCategoryService(IEnumerable<Domain.Entities.CategoryService> categoryServices)
        {
            try
            {
                var deleteTasks = categoryServices.Select(async cs =>
                {
                    try
                    {
                        await _unitOfWork.CategoryServiceRepository.DeleteAsync([cs.CategoryId, cs.ServiceId]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to delete CategoryService with ID: category-{cs.CategoryId}, service-{cs.ServiceId}. Error: {ex.Message}");
                    }
                });

                await Task.WhenAll(deleteTasks);

                if (deleteTasks.Any(t => t.IsFaulted))
                {
                    throw new Exception("Some category services could not be deleted successfully.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteById(Guid id)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var isExist = await _unitOfWork.ServiceRepository.GetByIdAsync(id) ?? throw new Exception("Service not found");

                var categoryServices = await _unitOfWork.CategoryServiceRepository.GetAsync(cs => cs.ServiceId.Equals(id));

                await DeleteCategoryService(categoryServices);

                await _unitOfWork.ServiceRepository.DeleteAsync(id);

                var check = await _unitOfWork.SaveAsync();
                if (check == 0)
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
                var services = await _unitOfWork.ServiceRepository.GetAsync(include: 
                    s => s.Include(c => c.CategoryServices).ThenInclude(cs => cs.Category)
                    //.Include(s => s.ServiceCustomCombos).ThenInclude(scc => scc.CustomCombo)
                    //.Include(s => s.ServiceTemplateCombos).ThenInclude(stc => stc.TemplateCombo)
                    //.Include(s => s.StoreServices).ThenInclude(ss => ss.Store)
                    ) ?? throw new Exception("Something was wrong!");

                return _mapper.Map<IEnumerable<ServiceResponse>>(services);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task HandleCategoryServiceUpdating(Guid serviceId, IList<Guid> categoryIds)
        {
            var existedCategoryServices = await _unitOfWork.CategoryServiceRepository.GetAsync(cs => cs.ServiceId.Equals(serviceId));

            // Lọc ra các CategoryService cần xóa
            await DeleteCategoryService(existedCategoryServices.Where(cs => !categoryIds.Contains(cs.CategoryId)));

            await InsertCategoryService(serviceId, categoryIds, existedCategoryServices);
        }


        public async Task Update(Guid id, ServiceUpdatingRequest updatingRequest)
        {
            try
            {
                var existedEntity = await _unitOfWork.ServiceRepository.GetByIdAsync(id) ?? throw new Exception("Service not found");

                var existedEntityName = await _unitOfWork.ServiceRepository.GetAsync(x => x.Name == updatingRequest.Name);

                if (existedEntityName != null && existedEntityName.Any() && existedEntityName.First().ID != id)
                    throw new Exception("Service already exists");

                _unitOfWork.BeginTransaction();

                var newEntity = _mapper.Map(updatingRequest, existedEntity);

                if (updatingRequest.NewImage != null)
                {
                    //await _firebaseService.DeleteImageAsync(newEntity.ImageUrl);

                    //Push new image to firebase
                    newEntity.ImageUrl = await _firebaseService.UploadFileAsync(updatingRequest.NewImage);
                }

                if (updatingRequest.CategoryIds != null)
                    await HandleCategoryServiceUpdating(id, updatingRequest.CategoryIds);

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
