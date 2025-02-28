using INBS.Domain.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.User.User
{
    public class UserRequest
    {
        public string FullName { get; set; } = string.Empty;
        public IFormFile? NewImage { get; set; }
        public string? ImageUrl { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
    }
}
