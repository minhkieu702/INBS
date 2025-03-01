using INBS.Application.DTOs.Authentication;
using INBS.Application.DTOs.User.User;

namespace INBS.Application.IServices
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> LoginCustomer(string phone, string password);
        Task<LoginResponse> LoginStaff(string username, string password);
        Task RegisterCustomer(UserRequest request, string password, string confirmPassword);
        Task<string> ResetPasswordStaff(string username, string newPassword, string confirmPassword);
        Task<string> ResetPasswordCustomer(string phone, string newPassword, string confirmPassword);
        Task<LoginResponse> VerifyOtpAsync(string phoneNumber, string otp);
        Task ChangeProfile(UserRequest requestModel);
    }
}
