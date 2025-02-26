using INBS.Application.DTOs.Authentication.Customer;
using INBS.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> LoginCustomer(string phone, string password);
        Task<LoginResponse> LoginStaff(string username, string password);
        Task RegisterCustomer(RegisterRequest request);
        Task<string> ResetPasswordStaff(string username, string newPassword, string confirmPassword);
        Task<string> ResetPasswordCustomer(string phone, string newPassword, string confirmPassword);
        Task<LoginResponse> VerifyOtpAsync(string phoneNumber, string otp);
    }
}
