using INBS.Application.DTOs.Service.Category;
using INBS.Application.Services;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service.Service
{
    public class CategoryServiceResponse
    {
        [Key]
        public Guid CategoryId { get; set; } = Guid.Empty;
        public virtual CategoryResponse? Category { get; set; }
        [Key]
        public Guid ServiceId { get; set; } = Guid.Empty;
        public virtual ServiceResponse? Service { get; set; }
    }
}
