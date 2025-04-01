using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Store
{
    public class StoreRequest
    {
        public string Province { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public float Latitude { get; set; }

        public float Longtitude { get; set; }

        public IFormFile? NewImage { get; set; }

        public int Status { get; set; }
    }
}
