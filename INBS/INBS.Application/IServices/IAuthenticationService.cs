using INBS.Application.DTOs.User.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IAuthenticationService
    {
        Task<string> SignInCustomer(string phone, string password);
        Task<string> SignInStaff(string username, string password);
        Task<string> SignUpCustomer(UserRequest request);
        Task ResetPassword(string phone);
        Task ConfirmOTP(string verificationId, string otp);
    }
}
