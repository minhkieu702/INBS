using AutoMapper;
using INBS.Application.DTOs.User.Artist;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class ArtistService(IUnitOfWork _unitOfWork, IFirebaseService _firebaseService, IMapper _mapper) : IArtistService
    {
        Task IArtistService.Create(ArtistRequest requestModel)
        {
            throw new NotImplementedException();
        }

        Task IArtistService.Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<ArtistResponse>> IArtistService.Get()
        {
            throw new NotImplementedException();
        }

        Task IArtistService.Update(Guid id, ArtistRequest requestModel)
        {
            throw new NotImplementedException();
        }
    }
}
