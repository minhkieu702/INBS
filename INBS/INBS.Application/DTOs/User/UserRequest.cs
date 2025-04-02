using INBS.Application.Common.MyJsonConverters;
using INBS.Domain.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.User
{
    public class UserRequest
    {
        public string FullName { get; set; } = string.Empty;
        public IFormFile? NewImage { get; set; }
        public string? ImageUrl { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }
    }
}
