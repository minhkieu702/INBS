using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.DTOs.NailDesignService;
using INBS.Application.DTOs.NailDesignServiceSelected;
using INBS.Application.DTOs.Service;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class NailDesignServiceService(IUnitOfWork _unitOfWork, IMapper _mapper, IAuthentication _authentication, IHttpContextAccessor _contextAccesstor) : INailDesignServiceService
    {
        public IQueryable<NailDesignServiceResponse> Get()
        {
            try
            {
                var role = _authentication.GetUserRoleFromHttpContext(_contextAccesstor.HttpContext);
                if (role == 2)
                {
                    return _unitOfWork.NailDesignServiceRepository.Query().ProjectTo<NailDesignServiceResponse>(_mapper.ConfigurationProvider);
            }
            return _unitOfWork.NailDesignServiceRepository.Query().Where(c => !c.IsDeleted).ProjectTo<NailDesignServiceResponse>(_mapper.ConfigurationProvider);
        }
            catch (Exception)
            {

                throw;
            }
        }



        public async Task UpdateTime(ServiceDurationRequest serviceDuration)
        {
            try
            {
                var count = await _unitOfWork.BookingRepository.Query()
                    .Select(c => c.CustomerSelected!.NailDesignServiceSelecteds.Where(c => c.NailDesignService!.ServiceId == serviceDuration.ServiceId)).CountAsync();

                var nailDesignService = await _unitOfWork.NailDesignServiceRepository.Query()
                    .FirstOrDefaultAsync(c => c.ServiceId == serviceDuration.ServiceId) ?? throw new Exception("This nail design service not found");

                nailDesignService.AverageDuration = (nailDesignService.AverageDuration * count + serviceDuration.Duration) / (count + 1);

                await _unitOfWork.NailDesignServiceRepository.UpdateAsync(nailDesignService);

                if (await _unitOfWork.SaveAsync() == 0)
                    throw new Exception("This action failed");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}