using INBS.Application.DTOs.Authentication;
using INBS.Application.DTOs.User.User;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IAdminService
    {
        Task<object> Create();

        Task<LoginResponse> Login(string username, string password);
    }
}
