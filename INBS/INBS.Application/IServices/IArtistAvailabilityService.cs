using INBS.Application.DTOs.User.Artist.ArtistAvailability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IArtistAvailabilityService
    {
        Task<IEnumerable<ArtistAvailabilityResponse>> Get();
        Task Create(ArtistAvailabilityRequest requestModel);
        Task Update(Guid id, ArtistAvailabilityRequest requestModel);
        Task Delete(Guid id);
    }
}
