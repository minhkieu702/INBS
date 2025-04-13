using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.DTOs.NailDesignService;
using INBS.Application.DTOs.NailDesignServiceSelected;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.IRepository;
using Microsoft.AspNetCore.Http;
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
    }
}