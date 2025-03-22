using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class NailDesignServiceSelectedService(IUnitOfWork _unitOfWork, IMapper _mapper) : INailDesignServiceSelectedService
    {
        public IQueryable<NailDesignServiceSelectedResponse> Get()
        {
			try
			{
				return _unitOfWork.NailDesignServiceSelectedRepository.Query().ProjectTo<NailDesignServiceSelectedResponse>(_mapper.ConfigurationProvider);
			}
			catch (Exception)
			{

				throw;
			}
        }
    }
}
