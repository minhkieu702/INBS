using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service
{
    public class ServiceUpdatingRequest
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public double Price { get; set; }

        public string? ImageUrl { get; set; }

        public IFormFile? NewImage { get; set; }

        public IList<Guid> CategoryIds { get; set; } = [];
    }
}
