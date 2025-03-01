using INBS.Application.DTOs.Customer;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface ICustomerService
    {
        Task<Customer> UpdatePreferencesAsync(Guid customerId, PreferencesRequest request);
        Task<string> GetPreferencesAsync(Guid customerId);
    }
}
