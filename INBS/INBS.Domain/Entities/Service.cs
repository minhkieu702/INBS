using INBS.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class Service : BaseEntity
    {
        public Service() : base()
        {
            Name = string.Empty;
            ImageUrl = string.Empty;
            StoreServices = [];
            ServiceCustomCombos = [];
            ServiceTemplateCombos = [];
            CategoryServices = [];
        }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string ImageUrl { get; set; }

        public double Price { get; set; }

        [InverseProperty(nameof(StoreService.Service))]
        public virtual ICollection<StoreService> StoreServices { get; set; }

        [InverseProperty(nameof(ServiceTemplateCombo.Service))]
        public virtual ICollection<ServiceTemplateCombo> ServiceTemplateCombos { get; set; }

        [InverseProperty(nameof(ServiceCustomCombo.Service))]
        public virtual ICollection<ServiceCustomCombo> ServiceCustomCombos { get; set; }

        [InverseProperty(nameof(CategoryService.Service))]
        public virtual ICollection<CategoryService> CategoryServices { get; set; }
    }
}
