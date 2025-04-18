﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.DTOs.DeviceToken;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.AspNetCore.Http;

namespace INBS.Application.Services
{
    public class DeviceTokenService(IUnitOfWork _unitOfWork, IMapper _mapper, IAuthentication _authentication, IHttpContextAccessor _contextAccessor) : IDeviceTokenService
    {

        public async Task AddDeviceToken(DeviceTokenRequest deviceTokenRequest)
        {
            try
            {
                var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
                var isDeviceTokenExist = await _unitOfWork.DeviceTokenRepository.GetAsync(query => query.Where(c => c.Token.Equals(deviceTokenRequest.Token) && c.UserId==userId));

                if (isDeviceTokenExist.Any())
                {
                    //throw new Exception("This device already have device token");
                    return;
                }
                var newToken = _mapper.Map<DeviceToken>(deviceTokenRequest);

                newToken.UserId = userId;

                newToken.CreatedAt = DateTime.UtcNow;

                await _unitOfWork.DeviceTokenRepository.InsertAsync(newToken);

                if (await _unitOfWork.SaveAsync() == 0)
                {
                    throw new Exception("This action failed");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task RemoveDeviceToken(string deviceToken)
        {
            try
            {
                var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);

                var existedToken = (await _unitOfWork.DeviceTokenRepository.GetAsync(c => c.Where(c => Equals(deviceToken, c.Token) && Equals(userId,c.UserId)))).FirstOrDefault() ??throw new Exception("Device token not found");

                await _unitOfWork.DeviceTokenRepository.DeleteAsync(existedToken.ID);

                if (await _unitOfWork.SaveAsync() == 0)
                {
                    throw new Exception("This action failed");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<DeviceTokenResponse> Get()
        {
            try
            {
                return _unitOfWork.DeviceTokenRepository.Query().ProjectTo<DeviceTokenResponse>(_mapper.ConfigurationProvider);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
