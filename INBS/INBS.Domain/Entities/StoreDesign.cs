using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class StoreDesign
    {
        public Guid StoreId { get; set; }
        [ForeignKey(nameof(StoreId))]
        [InverseProperty(nameof(Store.StoreDesigns))]
        public virtual Store? Store { get; set; }

        public Guid DesignId { get; set; }
        [ForeignKey(nameof(DesignId))]
        [InverseProperty(nameof(Design.StoreDesigns))]
        public virtual Design? Design { get; set; }
    }
}
