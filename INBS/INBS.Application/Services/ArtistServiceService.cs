using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.DTOs.ArtistService;
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
    public class ArtistServiceService(IUnitOfWork _unitOfWork, IMapper _mapper) : IArtistServiceService
    {
        public IQueryable<ArtistServiceResponse> Get()
        {
            try
            {
                return _unitOfWork.ArtistServiceRepository.Query().ProjectTo<ArtistServiceResponse>(_mapper.ConfigurationProvider);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
