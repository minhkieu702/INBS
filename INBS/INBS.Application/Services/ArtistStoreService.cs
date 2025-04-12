using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.DTOs.ArtistStore;
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
    public class ArtistStoreService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authentication, IHttpContextAccessor contextAccessor) : IArtistStoreService
    {
        public IQueryable<ArtistStoreResponse> GetAll()
        {
            var role = authentication.GetUserRoleFromHttpContext(contextAccessor.HttpContext);
            if (role == 2)
            {
                return unitOfWork.ArtistStoreRepository.Query().ProjectTo<ArtistStoreResponse>(mapper.ConfigurationProvider);
            }
            return unitOfWork.ArtistStoreRepository.Query().Where(c => !c.IsDeleted).ProjectTo<ArtistStoreResponse>(mapper.ConfigurationProvider);
        }
    }
}
