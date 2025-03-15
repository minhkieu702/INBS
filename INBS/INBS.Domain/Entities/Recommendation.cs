using INBS.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class Recommendation
    {
        public Guid DesignId { get; set; }
        [ForeignKey(nameof(DesignId))]
        [InverseProperty(nameof(Design.Recommendations))]
        public virtual Design? Design { get; set; }

        public DateTime GenerateAt { get; set; }

        public Guid CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(Customer.Recommendations))]
        public virtual Customer? Customer { get; set; }
    }
}
