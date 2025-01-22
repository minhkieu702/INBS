using INBS.Application.DTOs.Service.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IStoreService
    {
        Task<IEnumerable<StoreResponse>> Get();

        Task Create(StoreRequest request);

        Task Delete(Guid id);

        Task Update(Guid id, StoreRequest request);
    }
}
