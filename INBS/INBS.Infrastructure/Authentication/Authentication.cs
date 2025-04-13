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
        public string GenerateJwtToken(User user, int expirationInMinutes = 30)
        {
            var issuer = Environment.GetEnvironmentVariable("JWTSettings:Issuer");
            var audience = Environment.GetEnvironmentVariable("JWTSettings:Audience");
            var key = Environment.GetEnvironmentVariable("JWTSettings:Key");

            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT configuration is missing.");
            }

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.ID.ToString()),
                new (ClaimTypes.Role, user.Role.ToString())
            };

            var expiration = DateTime.UtcNow.AddMinutes(expirationInMinutes);

            var token = new JwtSecurityToken(issuer, audience, claims,
                notBefore: DateTime.UtcNow,
                expires: expiration,
                signingCredentials: credentials);

            return jwtSecurityTokenHandler.WriteToken(token);
        }

        public async Task<string> GenerateDefaultTokenAsync(User user)
        {
            return await Task.FromResult(GenerateJwtToken(user, expirationInMinutes: 30)); // Access token với thời gian sống 30 phút
        }

        public async Task<string> GenerateRefreshTokenAsync(User user)
        {
            return await Task.FromResult(GenerateJwtToken(user, expirationInMinutes: 20160)); // Refresh token với thời gian sống 14 ngày (20160 phút)
        }

        public Guid GetUserIdFromHttpContext(HttpContext httpContext)
        {
            Claim? userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (userIdClaim == null || string.IsNullOrWhiteSpace(userIdClaim.Value))
            {
                throw new UnauthorizedAccessException("You must login first");
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid User ID format in token.");
            }

            return userId;
        }

        public string HashedPassword(User user, string password)
        {
            try
            {
                var passwordHasher = new PasswordHasher<User>();

                var hashedPassword = passwordHasher.HashPassword(user, password);

                return hashedPassword;
            }
            catch (Exception ex)
            {
                throw new SecurityException("An error occurred while hashing the password.", ex);
            }
        }

        public bool VerifyPassword(User user, string password)
        {
            try
            {
                var passwordHasher = new PasswordHasher<User>();

                var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

                return result == PasswordVerificationResult.Success;
            }
            catch (Exception ex)
            {
                throw new SecurityException("An error occurred while hashing the password.", ex);
            }
        }

        public int GetUserRoleFromHttpContext(HttpContext httpContext)
        {
            Claim? roleString = httpContext.User.FindFirst(ClaimTypes.Role);


            if (roleString == null || string.IsNullOrWhiteSpace(roleString.Value))
            {
                return 0; // Default role if not found
            }

            if (!int.TryParse(roleString.Value, out var role))
            {
                throw new UnauthorizedAccessException("Invalid role format in token.");
            }

            return role;
        }
    }
}
