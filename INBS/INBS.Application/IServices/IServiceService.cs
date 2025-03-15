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
        Task Create(ServiceRequest service, IList<ServiceNailDesignRequest> serviceNailDesignRequests);
        Task Update(Guid id, ServiceRequest service, IList<ServiceNailDesignRequest> serviceNailDesignRequests);
        Task DeleteById(Guid id);
    }
}
