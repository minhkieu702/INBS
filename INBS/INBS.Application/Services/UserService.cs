using AutoMapper;
using INBS.Application.DTOs.User.User;
using INBS.Application.DTOs.User.User.Login;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
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
        private readonly IFirebaseService _firebaseService;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserService(IUnitOfWork unitOfWork, IAuthentication authentication,IFirebaseService firebaseService, IMapper mapper,IPasswordHasher<User> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _authentication = authentication;
            _firebaseService = firebaseService;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }
        public async Task<UserResponse> Register(UserRequest requestModel)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var existedUser = await _unitOfWork.UserRepository.GetAsync(x => x.Email == requestModel.Email);

                if (existedUser != null && existedUser.Any())
                    throw new Exception("This email is already registered");

                var newUser = _mapper.Map<User>(requestModel);

                newUser.PasswordHash = _passwordHasher.HashPassword(newUser, requestModel.Password);
                newUser.ImageUrl = requestModel.NewImage != null ? await _firebaseService.UploadFileAsync(requestModel.NewImage) : DEFAULT_IMAGE_URL;
                newUser.Notifications = new List<Notification>();

                await _unitOfWork.UserRepository.InsertAsync(newUser);

                if (await _unitOfWork.SaveAsync() == 0)
                    throw new Exception("User registration failed");

                _unitOfWork.CommitTransaction();
                return _mapper.Map<UserResponse>(newUser);
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task<UserResponse> Login(LoginRequest requestModel)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAsync(x => x.PhoneNumber == requestModel.PhoneNumber);

                if (user == null || !user.Any())
                    throw new Exception("Invalid phone number or password");

                var existingUser = user.First();

                var verifyPassword = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, requestModel.Password);

                if (verifyPassword != PasswordVerificationResult.Success)
                    throw new Exception("Invalid phone number or password");

                var accessToken = await _authentication.GenerateDefaultTokenAsync(existingUser);
                var refreshToken = await _authentication.GenerateRefreshTokenAsync(existingUser);

                return _mapper.Map<UserResponse>(existingUser);
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
