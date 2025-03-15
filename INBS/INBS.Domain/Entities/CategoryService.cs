using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class CategoryService
    {
        public int CategoryId { get; set; }

        public Guid ServiceId { get; set; }
        [ForeignKey(nameof(ServiceId))]
        [InverseProperty(nameof(Service.CategoryServices))]
        public virtual Service? Service { get; set; }
    }
}
