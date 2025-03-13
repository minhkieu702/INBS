using INBS.Application.DTOs.Customer;
using INBS.Application.DTOs.Preference;

namespace INBS.Application.IServices
{
    public interface ICustomerService
    {
        Task UpdatePreferencesAsync(PreferenceRequest request);
        IQueryable<CustomerResponse> Get();
    }
}
