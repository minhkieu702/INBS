using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.DTOs.Artist;
using INBS.Application.DTOs.Store;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Common;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.AspNetCore.Http;
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
    public class StoreService(IUnitOfWork _unitOfWork, IMapper _mapper, IFirebaseStorageService _firebaseService) : IStoreService
    {
        public async Task Create(StoreRequest modelRequest)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedEntity = await _unitOfWork.StoreRepository.GetAsync(c => c.Where(x => x.Address == modelRequest.Address));

                if (existedEntity != null && existedEntity.Any())
                    throw new Exception("This address was already used");
                var newEntity = _mapper.Map<Store>(modelRequest);

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

                existingEntity.IsDeleted = !existingEntity.IsDeleted;

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

        public IQueryable<StoreResponse> Get()
        {
            try
            {
                return _unitOfWork.StoreRepository.Query().ProjectTo<StoreResponse>(_mapper.ConfigurationProvider);
            }
            catch (Exception)
            {
                return Enumerable.Empty<StoreResponse>().AsQueryable();
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
