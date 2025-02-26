using INBS.Domain.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.User.User.Login
{
    public class RegisterRequest
    {
        public string FullName { get; set; } = string.Empty;
        public IFormFile? NewImage { get; set; }
        public string ImageUrl { get; set; } = Constants.DEFAULT_IMAGE_URL;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;
    }
}
