using INBS.Application.DTOs.ArtistStore;
using INBS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IArtistStoreService
    {
        IQueryable<ArtistStoreResponse> GetAll();
        Task Update(Guid id, ArtistStoreStatus artistStoreStatus);
    }
}
