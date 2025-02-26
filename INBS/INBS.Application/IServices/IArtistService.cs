using INBS.Application.DTOs.User.Artist;
using INBS.Application.DTOs.User.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IArtistService
    {
        Task<IEnumerable<ArtistResponse>> Get();
        Task<ArtistResponse> Create(ArtistRequest requestModel, UserRequest userRequest);
        Task Update(Guid id, ArtistRequest requestModel, UserRequest userRequest);
        Task Delete(Guid id);
    }
}
