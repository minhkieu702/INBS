using INBS.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class NailDesign
    {
        public Guid DesignId { get; set; }
        [ForeignKey(nameof(DesignId))]
        [InverseProperty(nameof(Design.NailDesigns))]
        public virtual Design? Design { get; set; }

        public int NailPosition { get; set; } //4, 8, 12, 16, 20

        public bool IsLeft { get; set; } //true - left, false - right

    }
}
