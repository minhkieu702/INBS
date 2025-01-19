using INBS.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Category() : base()
        {
            Name = string.Empty;
            Description = string.Empty;
            CategoryServices = [];
        }
        public string Name { get; set; }
        public string Description { get; set; }

        [InverseProperty(nameof(CategoryService.Category))]
        public virtual ICollection<CategoryService> CategoryServices { get; set; }
    }
}
