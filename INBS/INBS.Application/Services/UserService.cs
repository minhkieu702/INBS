using AutoMapper;
using INBS.Application.DTOs.User.User;
using INBS.Application.DTOs.User.User.Login;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Common;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class UserService : IUserService
    {
        private const string DEFAULT_IMAGE_URL = "https://your-default-image-url.com/default.png";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthentication _authentication;
        private readonly ISMSService _smsService;
        private readonly IFirebaseService _firebaseService;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IAuthentication authentication, ISMSService smsService, IFirebaseService firebaseService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _authentication = authentication;
            _smsService = smsService;
            _firebaseService = firebaseService;
            _mapper = mapper;
        }
        public async Task<UserResponse> Register(RegisterRequest requestModel)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedUser = await _unitOfWork.UserRepository.GetAsync(x => x.PhoneNumber == requestModel.PhoneNumber);

                if (existedUser != null && existedUser.Any())
                    throw new Exception("This phone number is already registered");

                var newUser = _mapper.Map<User>(requestModel);

                newUser.PasswordHash = _authentication.HashedPassword(newUser, requestModel.Password);

                newUser.ImageUrl = requestModel.NewImage != null ? await _firebaseService.UploadFileAsync(requestModel.NewImage) : Constants.DEFAULT_IMAGE_URL;

                var otpCode = _smsService.GenerateOtp();
                newUser.OtpCode = otpCode;
                newUser.OtpExpiry = DateTime.UtcNow.AddMinutes(5);

                await _unitOfWork.UserRepository.InsertAsync(newUser);

                if (await _unitOfWork.SaveAsync() == 0)
                    throw new Exception("User registration failed");

                await _smsService.SendOtpSmsAsync(newUser.PhoneNumber ?? string.Empty, otpCode);
                _unitOfWork.CommitTransaction();
                return _mapper.Map<UserResponse>(newUser);
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task<LoginResponse> Login(LoginRequest requestModel)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAsync(x => x.PhoneNumber == requestModel.PhoneNumber);

                if (user == null || !user.Any())
                    throw new Exception("Invalid phone number or password");

                var existingUser = user.First();
                if (!existingUser.IsVerified)
                    throw new Exception("Phone number is not verified. Please verify your OTP first.");

                var verifyPassword = _authentication.VerifyPassword(existingUser, requestModel.Password);
                if (!verifyPassword)
                    throw new Exception("Invalid phone number or password");

                var accessToken = await _authentication.GenerateDefaultTokenAsync(existingUser);
                var refreshToken = await _authentication.GenerateRefreshTokenAsync(existingUser);

                var response = _mapper.Map<LoginResponse>(existingUser);
                response.AccessToken = accessToken;
                response.RefreshToken = refreshToken;

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<LoginResponse> VerifyOtpAsync(VerifyOtpRequest requestModel)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(x => x.PhoneNumber == requestModel.PhoneNumber);

            if (user == null || !user.Any())
                throw new Exception("Phone number is not registered");

            var existingUser = user.First();

            if (existingUser.OtpCode != requestModel.OtpCode || existingUser.OtpExpiry < DateTime.UtcNow)
                throw new Exception("Invalid or expired OTP");

            existingUser.IsVerified = true;
            existingUser.OtpCode = null;
            existingUser.OtpExpiry = null;
            await _unitOfWork.UserRepository.UpdateAsync(existingUser);
            await _unitOfWork.SaveAsync();

            var accessToken = await _authentication.GenerateDefaultTokenAsync(existingUser);
            var refreshToken = await _authentication.GenerateRefreshTokenAsync(existingUser);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
