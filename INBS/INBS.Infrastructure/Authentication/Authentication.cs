using INBS.Application.Interfaces;
using INBS.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;

namespace INBS.Infrastructure.Authentication
{
    public class Authentication : IAuthentication
    {
        public async Task<string> GenerateRefreshTokenAsync(User user)
        {
            var issuer = Environment.GetEnvironmentVariable("Jwt:Issuer");
            var audience = Environment.GetEnvironmentVariable("Jwt:Audience");
            var key = Environment.GetEnvironmentVariable("Jwt:Key");

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? string.Empty));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>()
            {
                new (ClaimTypes.NameIdentifier, user.ID.ToString()),
                new (ClaimTypes.Name, user.Username.ToString()),
                new (ClaimTypes.Role, user.Role.ToString())
            };

            var expired = DateTime.UtcNow.AddDays(14);

            var token = new JwtSecurityToken(issuer, audience, claims, notBefore: DateTime.UtcNow, expires: expired, signingCredentials: credentials);
            return await Task.FromResult(jwtSecurityTokenHandler.WriteToken(token));
        }

        public async Task<string> GenerateDefaultTokenAsync(User user)
        {
            var issuer = Environment.GetEnvironmentVariable("Jwt:Issuer");
            var audience = Environment.GetEnvironmentVariable("Jwt:Audience");
            var key = Environment.GetEnvironmentVariable("Jwt:Key");

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? string.Empty));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new(ClaimTypes.Name, user.Username.ToString()),
                new(ClaimTypes.Role, user.Role.ToString())
            };

            var expired = DateTime.UtcNow.AddMinutes(30);

            var token = new JwtSecurityToken(issuer, audience, claims, notBefore: DateTime.UtcNow, expires: expired, signingCredentials: credentials);
            return await Task.FromResult(jwtSecurityTokenHandler.WriteToken(token));
        }

        public Guid GetUserIdFromHttpContext(HttpContext httpContext)
        {
            Claim? userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (userIdClaim == null || string.IsNullOrWhiteSpace(userIdClaim.Value))
            {
                throw new UnauthorizedAccessException("User ID claim not found in token.");
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid User ID format in token.");
            }

            return userId;
        }

        public string HashedPassword(string password)
        {
            try
            {
                var passwordHasher = new PasswordHasher<User>();

                var hashedPassword = passwordHasher.HashPassword(new User(), password);

                return hashedPassword;
            }
            catch (Exception ex)
            {
                throw new SecurityException("An error occurred while hashing the password.", ex);
            }
        }
    }
}
