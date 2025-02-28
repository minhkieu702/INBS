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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class StoreService(IUnitOfWork _unitOfWork, IMapper _mapper, IFirebaseService _firebaseService) : IStoreService
    {
        public async Task Create(StoreRequest modelRequest, ClaimsPrincipal user)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedEntity = await _unitOfWork.StoreRepository.GetAsync(x => x.Address == modelRequest.Address);

                if (existedEntity != null && existedEntity.Any())
                    throw new Exception("This address was already used");
                var newEntity = _mapper.Map<Store>(modelRequest);

                newEntity.AdminId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                    "E492D8F4-43EE-4AE2-BE26-6128E2D8C582");

                newEntity.ImageUrl = modelRequest.NewImage != null ? await _firebaseService.UploadFileAsync(modelRequest.NewImage) : Constants.DEFAULT_IMAGE_URL;

                newEntity.CreatedAt = DateTime.Now;

                await _unitOfWork.StoreRepository.InsertAsync(newEntity);

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

                //existingEntity.AdminId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                    //"7d9e01f9-d23a-403c-b496-097af797de02");

                if (request.NewImage != null)
                {
                    existingEntity.ImageUrl = await _firebaseService.UploadFileAsync(request.NewImage);
                }
                
                await _unitOfWork.StoreRepository.UpdateAsync(existingEntity);

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
    }
}
