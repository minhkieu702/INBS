using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class AccessoryCustomNailDesign
    {
        public Guid AccessoryId { get; set; }
        [ForeignKey(nameof(AccessoryId))]
        [InverseProperty(nameof(Accessory.AccessoryCustomNailDesigns))]
        public virtual Accessory? Accessory { get; set; }
        public Guid CustomNailDesignId { get; set; }
        [ForeignKey(nameof(CustomNailDesignId))]
        [InverseProperty(nameof(CustomNailDesign.AccessoryCustomNailDesigns))]
        public virtual CustomNailDesign? CustomNailDesign { get; set; }
    }
}
