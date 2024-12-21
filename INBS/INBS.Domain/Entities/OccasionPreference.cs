using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class OccasionPreference
    {
        public Guid CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(Customer.OccasionPreferences))]
        public virtual Customer? Customer { get; set; }

        public Guid OccasionId { get; set; }
        [ForeignKey(nameof(OccasionId))]
        [InverseProperty(nameof(Occasion.OccasionPreferences))]
        public virtual Occasion? Occasion { get; set; }
    }
}
