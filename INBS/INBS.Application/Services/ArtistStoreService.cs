using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.DTOs.ArtistStore;
using INBS.Application.IServices;
using INBS.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class ArtistStoreService(IUnitOfWork unitOfWork, IMapper mapper) : IArtistStoreService
    {
        public IQueryable<ArtistStoreResponse> GetAll()
        {
            return unitOfWork.ArtistStoreRepository.Query().ProjectTo<ArtistStoreResponse>(mapper.ConfigurationProvider);
        }
    }
}
