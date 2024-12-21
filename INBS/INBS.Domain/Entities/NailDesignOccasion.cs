using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class NailDesignOccasion
    {
        public Guid DesignId { get; set; }
        [ForeignKey(nameof(DesignId))]
        [InverseProperty(nameof(Design.NailDesignOccasions))]
        public virtual NailDesign? Design { get; set; }


        public Guid OccasionId { get; set; }
        [ForeignKey(nameof(OccasionId))]
        [InverseProperty(nameof(Occasion.NailDesignOccasions))]
        public virtual Occasion? Occasion { get; set; }
    }
}
