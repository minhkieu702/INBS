using AutoMapper;
using INBS.Domain.Enums;
using INBS.Application.DTOs.Authentication;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Common;
using INBS.Domain.Entities;
using INBS.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using INBS.Application.DTOs.User;
using Microsoft.EntityFrameworkCore;

namespace INBS.Application.Services
{
    public class AuthenticationService(IUnitOfWork _unitOfWork, IFirebaseService _firebaseService, IMapper _mapper, IAuthentication _authentication, ISMSService _smsService, IHttpContextAccessor _contextAccesstor) : IAuthenticationService
    {
        public async Task<bool> CheckPhoneNumberVerified(string phoneNumber)
        {
            phoneNumber = FormatPhoneNumber(phoneNumber);

            var user = (await _unitOfWork.UserRepository.GetAsync(x => x.Where(x => x.PhoneNumber == phoneNumber).Include(c => c.Customer))).FirstOrDefault() ?? throw new Exception("Phone number is not registered");
            
            var customer = user.Customer ?? throw new Exception("Phone number is not registered");


            if (!customer.IsVerified)
                throw new Exception("Phone number has not been verified");

            return true;
        }

        public async Task<LoginResponse> LoginCustomer(string phone, string password)
        {
            try
            {
                phone = FormatPhoneNumber(phone);
                var user = await _unitOfWork.UserRepository.GetAsync(x => x.Where(x => x.PhoneNumber == phone).Include(c => c.Customer));

                if (user == null || !user.Any())
                    throw new Exception("Invalid phone number or password");

                var existingUser = user.First();
                if (!existingUser.Customer!.IsVerified)
                    throw new Exception("Phone number is not verified. Please verify your OTP first.");

                if (existingUser.IsDeleted)
                    throw new Exception("This account has been already blocked");

                var verifyPassword = _authentication.VerifyPassword(existingUser, password);
                if (!verifyPassword)
                    throw new Exception("Invalid phone number or password");

                var accessToken = await _authentication.GenerateDefaultTokenAsync(existingUser);
                var refreshToken = await _authentication.GenerateRefreshTokenAsync(existingUser);

                return new LoginResponse { AccessToken = accessToken, RefreshToken = refreshToken };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LoginResponse> LoginStaff(string username, string password)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAsync(x => x
                    .Include(x => x.Artist)
                    .Where(x => x.Artist!.Username == username));

                if(user == null || !user.Any())
                    throw new Exception("Invalid username or password");

                var existingUser = user.First();

                if (existingUser.IsDeleted)
                    throw new Exception("This account has been already blocked");

                var verifyPassword = _authentication.VerifyPassword(existingUser, password);

                if (!verifyPassword)
                    throw new Exception("Invalid username or password");

                return new LoginResponse
                {
                    AccessToken = await _authentication.GenerateDefaultTokenAsync(existingUser),
                    RefreshToken = await _authentication.GenerateRefreshTokenAsync(existingUser)
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task IsUniquePhoneNumber(string phoneNumber, Guid? userId = null)
        {
            var artist = await _unitOfWork.UserRepository.GetAsync(c => c.Where(c => c.ID != userId && c.PhoneNumber == phoneNumber));
            if (artist.Any())
            {
                throw new Exception("Phone number already exists");
            }
        }
        private string FormatPhoneNumber(string phone)
        {
            if (!string.IsNullOrEmpty(phone) && phone.StartsWith("0"))
            {
                return "+84" + phone.Substring(1);
            }
            return phone;
        }
        private void IsPasswordMatching(string password, string confirmPassword)
        {
            if (password != confirmPassword)
                throw new Exception("Password and confirm password do not match");
        }

        private async Task<string> SendOtp(User user)
        {
            var otpCode = _smsService.GenerateOtp();
            await _smsService.SendOtpSmsAsync(user.PhoneNumber ?? string.Empty, otpCode);
            return otpCode;
        }

        public async Task ChangeProfile(UserRequest requestModel)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                
                var userId = _authentication.GetUserIdFromHttpContext(_contextAccesstor.HttpContext);
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                
                if (user == null)
                    throw new Exception("User not found");

                _mapper.Map(requestModel, user);

                if (requestModel.NewImage != null)
                    user.ImageUrl = await _firebaseService.UploadFileAsync(requestModel.NewImage);
                
                await _unitOfWork.UserRepository.UpdateAsync(user);

                if (await _unitOfWork.SaveAsync() == 0)
                    throw new Exception("Profile update failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task RegisterCustomer(UserRequest requestModel, string password, string confirmPassword)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                requestModel.PhoneNumber = FormatPhoneNumber(requestModel.PhoneNumber);

                IsPasswordMatching(password, confirmPassword);

                await IsUniquePhoneNumber(requestModel.PhoneNumber);

                var newUser = _mapper.Map<User>(requestModel);

                newUser.CreatedAt = DateTime.Now;

                newUser.PasswordHash = _authentication.HashedPassword(newUser, password);

                if (requestModel.NewImage != null)
                    await _firebaseService.UploadFileAsync(requestModel.NewImage);

                newUser.Role = (int)Role.Customer;

                //var otpCode = await SendOtp(newUser);

                var customer = new Customer 
                {
                    ID = newUser.ID,
                    IsVerified = true,
                    //OtpCode = otpCode,
                    OtpExpiry = DateTime.UtcNow.AddMinutes(5)
                };

                await _unitOfWork.CustomerRepository.InsertAsync(customer);

                await _unitOfWork.UserRepository.InsertAsync(newUser);

                if (await _unitOfWork.SaveAsync() == 0)
                    throw new Exception("User registration failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task<string> ResetPasswordCustomer(string phone, string newPassword, string confirmPassword)
        {
            try
            {
                IsPasswordMatching(newPassword, confirmPassword);

                phone = FormatPhoneNumber(phone);

                var user = (await _unitOfWork.UserRepository.GetAsync(x => x.Where(x => x.PhoneNumber == phone).Include(c => c.Customer))).FirstOrDefault();

                if (user == null)
                    throw new Exception("Phone number is not registered");

                user.PasswordHash = _authentication.HashedPassword(user, newPassword);

                var otp = await SendOtp(user);

                var customer = user.Customer ?? throw new Exception("Customer not found");
                customer.OtpCode = otp;
                customer.OtpExpiry = DateTime.UtcNow.AddMinutes(5);

                await _unitOfWork.UserRepository.UpdateAsync(user);

                return user.PhoneNumber!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> ResetPasswordStaff(string username, string newPassword, string confirmPassword)
        {
            try
            {
                IsPasswordMatching(newPassword, confirmPassword);

                var user = (await _unitOfWork.UserRepository.GetAsync(x => x
                   .Include(x => x.Artist)
                   .Where(x => x.Artist!.Username == username))).FirstOrDefault() 
                   ?? throw new Exception("Username not found");
                
                user.PasswordHash = _authentication.HashedPassword(user, newPassword);

                await _unitOfWork.UserRepository.UpdateAsync(user);

                return user.PhoneNumber!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<LoginResponse> VerifyOtpAsync(string phoneNumber, string otp)
        {
            phoneNumber = FormatPhoneNumber(phoneNumber);

            var user = (await _unitOfWork.UserRepository.GetAsync(query => query
                .Where(x => x.PhoneNumber == phoneNumber)
                .Include(x => x.Customer))).FirstOrDefault() 
                ?? throw new Exception("Phone number is not registered");
            
            var customer = user.Customer ?? throw new Exception("Phone number is not registered");

            if (customer.OtpCode != otp || customer.OtpExpiry < DateTime.UtcNow)
                throw new Exception("Invalid or expired OTP");

            customer.IsVerified = true;
            customer.OtpCode = null;
            customer.OtpExpiry = null;

            await _unitOfWork.CustomerRepository.UpdateAsync(customer);

            await _unitOfWork.SaveAsync();

            return new LoginResponse
            {
                AccessToken = await _authentication.GenerateDefaultTokenAsync(user),
                RefreshToken = await _authentication.GenerateRefreshTokenAsync(user)
            };
        }

        public async Task ResendOtpAsync(string phoneNumber)
        {
            phoneNumber = FormatPhoneNumber(phoneNumber);

            var user = (await _unitOfWork.UserRepository.GetAsync(query => query
                .Where(x => x.PhoneNumber == phoneNumber)
                .Include(x => x.Customer))).FirstOrDefault()
                ?? throw new Exception("Phone number is not registered");

            var customer = user.Customer ?? throw new Exception("Phone number is not registered");

            if (customer.OtpExpiry >= DateTime.UtcNow)
                throw new Exception("OTP is still valid, please use the existing OTP");

            customer.OtpCode = await SendOtp(user);
            customer.OtpExpiry = DateTime.UtcNow.AddMinutes(5);
            
            await _unitOfWork.CustomerRepository.UpdateAsync(customer);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    id = _authentication.GetUserIdFromHttpContext(_contextAccesstor.HttpContext);
                }

                var result = await _unitOfWork.UserRepository.GetByIdAsync(id) ?? throw new Exception("This user not found");

                await _unitOfWork.UserRepository.DeleteAsync(id);

                if (await _unitOfWork.SaveAsync() <= 0)
                    throw new Exception("Action failed");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
