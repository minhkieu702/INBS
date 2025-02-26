using AutoMapper;
using INBS.Application.DTOs.User.User;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class AuthenticationService(IUnitOfWork _unitOfWork, IFirebaseService _firebaseService, IMapper _mapper) : IAuthenticationService
    {
        public Task ConfirmOTP(string verificationId, string otp)
        {
            throw new NotImplementedException();
        }

        public Task ResetPassword(string phone)
        {
            throw new NotImplementedException();
        }

        public Task<string> SignInCustomer(string phone, string password)
        {
            throw new NotImplementedException();
        }

        public Task<string> SignInStaff(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<string> SignUpCustomer(UserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
