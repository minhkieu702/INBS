using AutoMapper;
using INBS.Application.DTOs.Store;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Common;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class StoreService(IUnitOfWork _unitOfWork, IMapper _mapper, IFirebaseService _firebaseService) : IStoreService
    {
        public async Task Create(StoreRequest modelRequest)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedEntity = await _unitOfWork.StoreRepository.GetAsync(x => x.Address == modelRequest.Address);

                if (existedEntity != null && existedEntity.Any())
                    throw new Exception("This address was already used");
                var newEntity = _mapper.Map<Store>(modelRequest);

                newEntity.ImageUrl = modelRequest.NewImage != null ? await _firebaseService.UploadFileAsync(modelRequest.NewImage) : Constants.DEFAULT_IMAGE_URL;

                newEntity.CreatedAt = DateTime.Now;

                await _unitOfWork.StoreRepository.InsertAsync(newEntity);

                var storeServices = await _unitOfWork.StoreServiceRepository.GetAsync(cs => cs.ServiceId.Equals(newEntity.ID));

                var storeDesigns = await _unitOfWork.StoreDesignRepository.GetAsync(cd => cd.DesignId.Equals(newEntity.ID));

                await Task.WhenAll(
                    InsertStoreService(newEntity.ID, modelRequest.ServiceIds, storeServices),
                    InsertStoreDesign(newEntity.ID, modelRequest.DesignIds, storeDesigns));

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

        private async Task InsertStoreService(Guid iD, IList<Guid> serviceIds, IEnumerable<Domain.Entities.StoreService> existedStoreServices)
        {
            if (serviceIds ==null || !serviceIds.Any()) return;
            var existingServiceIds = existedStoreServices.Select(ss => ss.ServiceId).ToHashSet();

            var list = new List<Domain.Entities.StoreService>();

            foreach (var serviceId in serviceIds)
            {
                if (existingServiceIds.Contains(serviceId)) continue;

                try
                {
                    if (await _unitOfWork.ServiceRepository.GetByIdAsync(serviceId) == null) continue;

                    var storeService = new Domain.Entities.StoreService
                    {
                        ServiceId = serviceId,
                        StoreId = iD
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to process StoreService with serviceId: {serviceId}, Error: {ex.Message}");
                }

                await _unitOfWork.StoreServiceRepository.InsertRangeAsync(list);
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existingEntity = await _unitOfWork.StoreRepository.GetByIdAsync(id) ?? throw new Exception("Store not found");

                existingEntity.IsDeleted = true;

                await _unitOfWork.StoreRepository.UpdateAsync(existingEntity);

                if (await _unitOfWork.SaveAsync() == 0)
                    throw new Exception("Delete store failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task<IEnumerable<StoreResponse>> Get()
        {
            try
            {
                var result = await _unitOfWork.StoreRepository.GetAsync(include:
                    query => query.Where(s => !s.IsDeleted)
                    .Include(s => s.StoreServices.Where(ss => ss.Service != null && !ss.Service.IsDeleted)).ThenInclude(ss => ss.Service)
                    //.Include(s => s.StoreDesigns.Where(sd => sd.Design != null && !sd.Design.IsDeleted)).ThenInclude(sd => sd.Design)
                    //.Include(s => s.Artists.Where(a => !a.IsDeleted))
                    //.Include(s => s.Admin)
                    );

                return _mapper.Map<IEnumerable<StoreResponse>>(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Update(Guid id, StoreRequest request)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existingEntity = await _unitOfWork.StoreRepository.GetByIdAsync(id) ?? throw new Exception("Store not found");

                _mapper.Map(request, existingEntity);

                if (request.NewImage != null)
                {
                    existingEntity.ImageUrl = await _firebaseService.UploadFileAsync(request.NewImage);
                }
                
                await _unitOfWork.StoreRepository.UpdateAsync(existingEntity);

                await Task.WhenAll(
                    HandleStoreDesignUpdating(id, request.DesignIds),
                    HandleStoreServiceUpdating(id, request.ServiceIds)
                    );

                if (await _unitOfWork.SaveAsync() == 0)
                    throw new Exception("Update store failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        private async Task HandleStoreServiceUpdating(Guid storeId, IList<Guid> serviceIds)
        {
            if (serviceIds == null || !serviceIds.Any()) return;

            var existedStoreServices = await _unitOfWork.StoreServiceRepository.GetAsync(ss => ss.StoreId == storeId);

            _unitOfWork.StoreServiceRepository.DeleteRange(existedStoreServices.Where(c => !serviceIds.Contains(c.ServiceId)));

            await InsertStoreService(storeId, serviceIds, existedStoreServices);
        }

        private async Task HandleStoreDesignUpdating(Guid storeId, IList<Guid> designIds)
        {
            if (designIds == null || !designIds.Any()) return;

            var existedStoreDesigns = await _unitOfWork.StoreDesignRepository.GetAsync(ss => ss.StoreId == storeId);

            _unitOfWork.StoreDesignRepository.DeleteRange(existedStoreDesigns.Where(c => !designIds.Contains(c.DesignId)));

            await InsertStoreDesign(storeId, designIds, existedStoreDesigns);
        }

        private async Task InsertStoreDesign(Guid storeId, IList<Guid> designIds, IEnumerable<StoreDesign> existedStoreDesigns)
        {
            if(designIds == null || !designIds.Any()) return ;

            var existingDesignIds = existedStoreDesigns.Select(c => c.DesignId).ToHashSet();

            var list = new List<StoreDesign>();

            foreach (var designId in designIds)
            {
                if (existingDesignIds.Contains(designId)) continue;

                try
                {
                    if (_unitOfWork.DesignRepository.GetByID(designId) == null) continue;

                    list.Add(new StoreDesign
                    {
                        DesignId = designId,
                        StoreId = storeId
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to process StoreDesign with DesignId: {designId}, Error: {ex.Message}");
                }
            }

            await _unitOfWork.StoreDesignRepository.InsertRangeAsync(list);
        }
    }
}
