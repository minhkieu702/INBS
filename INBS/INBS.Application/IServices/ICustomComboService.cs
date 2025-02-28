using INBS.Application.DTOs.Design.Accessory;
using INBS.Application.DTOs.Service.CustomCombo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface ICustomComboService
    {
        Task<IEnumerable<CustomComboResponse>> Get();
        Task Create(CustomComboRequest request, IList<Guid> serviceCustomCombos);
        Task Update(Guid id, CustomComboRequest request, IList<Guid> serviceCustomCombos);
        Task Delete(Guid id);
    }
}
