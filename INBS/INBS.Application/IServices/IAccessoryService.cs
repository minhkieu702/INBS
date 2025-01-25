using INBS.Application.DTOs.Design.Accessory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IAccessoryService
    {
        Task<IEnumerable<AccessoryResponse>> Get();
        Task Create(AccessoryRequest requestModel);
        Task Update(Guid id, AccessoryRequest requestModel);
        Task Delete(Guid id);
    }
}
