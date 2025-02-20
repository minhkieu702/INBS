using INBS.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Interfaces
{
    public interface IAuthentication
    {
        string HashedPassword(string password);
        Task<string> GenerateDefaultTokenAsync(User user);
        Guid GetUserIdFromHttpContext(HttpContext httpContext);
        Task<string> GenerateRefreshTokenAsync(User user);
    }
}
