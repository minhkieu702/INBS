using INBS.Application.DTOs.Common;
using INBS.Application.DTOs.Store;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service.Service
{
    public class ServiceResponse : BaseEntity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public string ImageUrl { get; set; }

        public double Price { get; set; }

        public ICollection<CategoryServiceResponse> CategoryServices { get; set; } = [];

        public ICollection<StoreServiceResponse> StoreServices { get; set; } = [];
    }
}
