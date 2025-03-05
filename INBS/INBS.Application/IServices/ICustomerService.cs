using INBS.Application.DTOs.Common.Preference;
using INBS.Application.DTOs.User.Customer;

namespace INBS.Application.IServices
{
    public interface ICustomerService
    {
        Task UpdatePreferencesAsync(PreferenceRequest request);
        Task<IEnumerable<CustomerResponse>> Get();
    }
}
