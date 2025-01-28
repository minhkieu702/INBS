using INBS.Application.DTOs.Service.CustomCombo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface ICustomComboService
    {
        Task<CustomComboResponse> Get();
        Task Create(CustomComboRequest request);
        Task Update(Guid id, CustomComboRequest request);
        Task Delete(Guid id);
    }
}
