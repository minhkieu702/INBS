using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class AccessoryCustomDesign
    {
        public Guid AccessoryId { get; set; }
        [ForeignKey(nameof(AccessoryId))]
        [InverseProperty(nameof(Accessory.AccessoryCustomDesigns))]
        public virtual Accessory? Accessory { get; set; }
        public Guid CustomDesignId { get; set; }
        [ForeignKey(nameof(CustomDesignId))]
        [InverseProperty(nameof(CustomDesign.AccessoryCustomDesigns))]
        public virtual CustomDesign? CustomDesign { get; set; }
    }
}
