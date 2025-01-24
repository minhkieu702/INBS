using INBS.Application.DTOs.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IStoreService
    {
        Task<IEnumerable<StoreResponse>> Get();

        Task Create(StoreRequest request, ClaimsPrincipal user);

        Task Delete(Guid id);

        Task Update(Guid id, StoreRequest request);
    }
}
