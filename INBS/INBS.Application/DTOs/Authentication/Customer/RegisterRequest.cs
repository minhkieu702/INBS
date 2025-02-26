using INBS.Domain.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Authentication.Customer
{
    public class RegisterRequest
    {
        public string FullName { get; set; } = string.Empty;
        public IFormFile? NewImage { get; set; }
        public string ImageUrl { get; set; } = Constants.DEFAULT_IMAGE_URL;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
