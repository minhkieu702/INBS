using INBS.Application.DTOs.User.User;
using INBS.Application.DTOs.User.User.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IUserService
    {
        Task<UserResponse> Register(UserRequest requestModel);
        Task<LoginResponse> Login(LoginRequest requestModel);
        Task<LoginResponse> VerifyOtpAsync(VerifyOtpRequest requestModel);
    }
}
