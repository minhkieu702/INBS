using INBS.Application.DTOs.Service.Service;
using INBS.Domain.Entities;
using INBS.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service.Category
{
    public class CategoryResponse : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<CategoryServiceResponse> CategoryServices { get; set; }
    }
}
