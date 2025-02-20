using INBS.Application.Interfaces;
using INBS.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
                new (ClaimTypes.Email, user.Email.ToString()),
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
                new(ClaimTypes.Email, user.Email.ToString()),
                new(ClaimTypes.Role, user.Role.ToString())
            };

            var expired = DateTime.UtcNow.AddMinutes(30);

            var token = new JwtSecurityToken(issuer, audience, claims, notBefore: DateTime.UtcNow, expires: expired, signingCredentials: credentials);
            return await Task.FromResult(jwtSecurityTokenHandler.WriteToken(token));
        }

        public Guid GetUserIdFromHttpContext(HttpContext httpContext)
    {
        if (!httpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            throw new UnauthorizedAccessException("Authorization header is missing.");
        }

        if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException("Invalid Authorization header format.");
        }

        string jwtToken = authorizationHeader.ToString()[7..]; // Extract token part

        var tokenHandler = new JwtSecurityTokenHandler();
        if (!tokenHandler.CanReadToken(jwtToken))
        {
            throw new SecurityTokenException("Invalid JWT token format.");
        }

        try
        {
            var token = tokenHandler.ReadJwtToken(jwtToken);
            var idClaim = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier || claim.Type == "sub" || claim.Type == "user_id");

                if (idClaim == null || string.IsNullOrWhiteSpace(idClaim.Value))
            {
                throw new SecurityTokenException("User ID claim not found in token.");
            }

            return Guid.Parse(idClaim.Value);
        }
        catch (FormatException)
        {
            throw new SecurityTokenException("Invalid GUID format in token.");
        }
        catch (Exception ex)
        {
            throw new SecurityTokenException($"Error parsing token: {ex.Message}");
        }
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
