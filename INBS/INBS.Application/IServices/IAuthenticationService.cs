using INBS.Application.DTOs.Authentication;
using INBS.Application.DTOs.User;

namespace INBS.Application.IServices
{
    public interface IAuthenticationService
    {
        Task<bool> CheckPhoneNumberVerified(string phoneNumber);
        Task<LoginResponse> LoginCustomer(string phone, string password);
        Task<LoginResponse> LoginStaff(string username, string password);
        Task RegisterCustomer(UserRequest request, string password, string confirmPassword);
        Task<string> ResetPasswordStaff(string username, string newPassword, string confirmPassword);
        Task<LoginResponse> ResetPasswordCustomer(string phone,string otp, string newPassword, string confirmPassword);
        Task<LoginResponse> VerifyOtpAsync(string phoneNumber, string otp);
        Task ResendOtpAsync(string phoneNumber);
        Task ChangeProfile(UserRequest requestModel);
        Task Delete(Guid? id);
    }
}
