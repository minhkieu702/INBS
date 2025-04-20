using INBS.Application.DTOs.Customer;
using INBS.Application.DTOs.Preference;
using INBS.Domain.Entities;
using static INBS.Application.Services.CustomerService;

namespace INBS.Application.IServices
{
    public interface ICustomerService
    {
        Task UpdatePreferencesAsync(CustomerPreferenceRequest request);
        IQueryable<CustomerResponse> Get();
    }
}
