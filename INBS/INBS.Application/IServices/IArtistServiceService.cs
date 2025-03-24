using INBS.Application.DTOs.ArtistService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IArtistServiceService
    {
        IQueryable<ArtistServiceResponse> Get();
    }
}
