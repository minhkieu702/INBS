using AutoMapper;
using INBS.Application.DTOs.Design.Accessory;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Common;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class AccessoryService(IUnitOfWork _unitOfWork, IFirebaseService _firebaseService, IMapper _mapper) : IAccessoryService
    {
        public async Task Create(AccessoryRequest requestModel)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedEntity = await _unitOfWork.AccessoryRepository.GetAsync(x => x.Name == requestModel.Name);

                if (existedEntity != null && existedEntity.Any())
                    throw new Exception("This accessory was already existed");

                var newEntity = _mapper.Map<Accessory>(requestModel);

                newEntity.ImageUrl = requestModel.NewImage != null ? await _firebaseService.UploadFileAsync(requestModel.NewImage) : Constants.DEFAULT_IMAGE_URL;

                newEntity.CreatedAt = DateTime.Now;

                await _unitOfWork.AccessoryRepository.InsertAsync(newEntity);

                if (await _unitOfWork.SaveAsync() == 0)
                    throw new Exception("Create accessory failed");

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
                var existedEntity = await _unitOfWork.AccessoryRepository.GetByIdAsync(id) ?? throw new Exception("This accessory was already existed");

                await _unitOfWork.AccessoryRepository.DeleteAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AccessoryResponse>> Get()
        {
            try
            {
                var existedEntity = await _unitOfWork.AccessoryRepository.GetAsync(include: query => query.Where(c => !c.IsDeleted)) ?? throw new Exception("This this action failed");

                return _mapper.Map<IEnumerable<AccessoryResponse>>(existedEntity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(Guid id, AccessoryRequest requestModel)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedEntity = await _unitOfWork.AccessoryRepository.GetByIdAsync(id) ?? throw new Exception("This id is not existed"); ;

                _mapper.Map(requestModel, existedEntity);

                if (requestModel.NewImage != null)
                {
                    existedEntity.ImageUrl = await _firebaseService.UploadFileAsync(requestModel.NewImage);
                }

                await _unitOfWork.AccessoryRepository.UpdateAsync(existedEntity);

                if (await _unitOfWork.SaveAsync() == 0)
                    throw new Exception("This action failed");

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
