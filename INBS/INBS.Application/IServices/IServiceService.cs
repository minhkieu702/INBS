using INBS.Application.DTOs.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceResponse>> Get();
        Task<ServiceResponse> GetById(int id);
        Task Create(ServiceRequest category);
        Task Update(Guid id, ServiceRequest category);
        Task DeleteById(Guid id);
    }
}
