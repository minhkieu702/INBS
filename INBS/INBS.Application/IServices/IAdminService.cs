using INBS.Application.DTOs.Authentication;

namespace INBS.Application.IServices
{
    public interface IAdminService
    {
        Task<object> Create();

        Task<LoginResponse> Login(string username, string password);
    }
}
