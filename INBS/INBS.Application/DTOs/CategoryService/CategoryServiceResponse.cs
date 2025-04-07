using INBS.Application.DTOs.Service;
using INBS.Application.Services;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.CategoryService
{
    public class CategoryServiceResponse
    {
        [Key]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        [Key]
        public Guid ServiceId { get; set; } = Guid.Empty;
        public virtual ServiceResponse? Service { get; set; }

        public virtual object? Data { get; set; }
    }
}
