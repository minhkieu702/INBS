using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INBS.Domain.Entities.Common;

namespace INBS.Domain.Entities
{
    public class Accessory : BaseEntity
    {
        public Accessory() : base()
        {
            AccessoryCustomDesigns = [];
            Name = string.Empty;
            ImageUrl = string.Empty;
        }

        public string Name { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }

        [InverseProperty(nameof(AccessoryCustomDesign.Accessory))]
        public virtual ICollection<AccessoryCustomDesign> AccessoryCustomDesigns { get; set; }
    }
}
