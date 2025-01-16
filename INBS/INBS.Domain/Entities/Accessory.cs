using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class Accessory
    {
        public Accessory()
        {
            AccessoryCustomDesigns = [];
            ID = Guid.NewGuid();
            Name = string.Empty;
            ImageUrl = string.Empty;
        }

        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }

        [InverseProperty(nameof(AccessoryCustomDesign.Accessory))]
        public virtual ICollection<AccessoryCustomDesign> AccessoryCustomDesigns { get; set; }
    }
}
