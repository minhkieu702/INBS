using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service.Service
{
    public class ServiceRequest
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public double Price { get; set; }

        public string? ImageUrl { get; set; }

        public IFormFile? NewImage { get; set; }

        public bool IsAdditional { get; set; }

        public IList<int> CategoryIds { get; set; } = [];
    }
}
