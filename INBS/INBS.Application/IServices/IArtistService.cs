using INBS.Application.DTOs.User.Artist;
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
        Task Create(ArtistRequest requestModel);
        Task Update(Guid id, ArtistRequest requestModel);
        Task Delete(Guid id);
    }
}
