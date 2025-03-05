using AutoMapper;
using INBS.Application.DTOs.Authentication;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using INBS.Domain.Enums;
using INBS.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Services
{
    public class AdminService(IUnitOfWork unitOfWork, IAuthentication authentication) : IAdminService
    {
        public async Task<object> Create()
        {
            try
            {
                var check = await unitOfWork.UserRepository.GetAsync(x => x.Username == "admin" && x.Role == (int)Role.Admin);

                if (check.Any())
                {
                    throw new Exception("Admin already exist");
                }
                var user = new User
                {
                    Username = "admin",
                    Role = (int)Role.Admin,
                    CreatedAt = DateTime.Now,
                    IsVerified = true,
                    DateOfBirth = new DateOnly(1990, 1, 1)
                };

                user.PasswordHash = authentication.HashedPassword(user, "admin");

                await unitOfWork.UserRepository.InsertAsync(user);

                 await unitOfWork.AdminRepository.InsertAsync(new Admin
                {
                    ID = user.ID
                });

                if (unitOfWork.Save() == 0)
                    throw new Exception("Create admin failed");

                return new
                {
                    username = "admin",
                    password = "admin"
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<LoginResponse> Login(string username, string password)
        {
            try
            {
                var user = await unitOfWork.UserRepository.GetAsync(x => x.Username == username && x.Role == (int)Role.Admin);

                if (!user.Any() || user.First() == null)
                {
                    throw new Exception("This username not found");
                }

                if (!authentication.VerifyPassword(user.First(), password))
                    throw new Exception("Password is incorrect");

                return new LoginResponse()
                {
                    AccessToken = await authentication.GenerateDefaultTokenAsync(user.First()),
                    RefreshToken = await authentication.GenerateRefreshTokenAsync(user.First())
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
