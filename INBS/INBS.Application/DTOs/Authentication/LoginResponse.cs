using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Authentication
{
    public class LoginResponse
    {
        /// <summary>
        /// Access token and refresh token response
        /// </summary>
        public LoginResponse()
        {
            AccessToken = string.Empty;
            RefreshToken = string.Empty;
        }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
