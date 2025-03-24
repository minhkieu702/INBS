using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.DTOs.NailDesignService;
using INBS.Application.DTOs.NailDesignServiceSelected;
using INBS.Application.IServices;
using INBS.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class NailDesignServiceService(IUnitOfWork _unitOfWork, IMapper _mapper) : INailDesignServiceService
    {
        public IQueryable<NailDesignServiceResponse> Get()
        {
            try
            {
                return _unitOfWork.NailDesignServiceRepository.Query().ProjectTo<NailDesignServiceResponse>(_mapper.ConfigurationProvider);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}