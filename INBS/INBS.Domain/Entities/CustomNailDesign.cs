using INBS.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class CustomNailDesign : BaseEntity
    {
        public CustomNailDesign() : base()
        {
            AccessoryCustomNailDesigns = [];
        }
        public Guid CustomDesignId { get; set; }
        [ForeignKey(nameof(CustomDesignId))]
        [InverseProperty(nameof(CustomDesign.CustomNailDesigns))]
        public virtual CustomDesign? CustomDesign { get; set; }


        [InverseProperty(nameof(AccessoryCustomNailDesign.CustomNailDesign))]
        public virtual ICollection<AccessoryCustomNailDesign> AccessoryCustomNailDesigns { get; set; }
    }
}
