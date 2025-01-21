using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service
{
    public class ServiceRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public double Price { get; set; }

        public IFormFile? Image { get; set; }

        public IList<Guid> CategoryIds { get; set; } = [];
    }
}
