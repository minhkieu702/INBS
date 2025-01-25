using INBS.Domain.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Design.Accessory
{
    public class AccessoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public IFormFile? NewImage { get; set; }
        public string? ImageUrl { get; set; } = Constants.DEFAULT_IMAGE_URL;
    }
}
