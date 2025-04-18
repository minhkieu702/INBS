using INBS.Application.DTOs.Customer;
using INBS.Application.DTOs.Preference;
using static INBS.Application.Services.CustomerService;

namespace INBS.Application.IServices
{
    public interface ICustomerService
    {
        Task UpdatePreferencesAsync(PreferenceRequest request);
        IQueryable<CustomerResponse> Get();
        Task<SkinTone> DetectSkinToneFromImage(Stream imageStream);

    }
}
