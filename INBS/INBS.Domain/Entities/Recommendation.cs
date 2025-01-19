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
    public class Recommendation : BaseEntity
    {
        public Recommendation() : base()
        {
        }

        public string? RecommendedDesigns { get; set; }

        public string? RecommendedTimeSlots { get; set; }

        public string? Artists { get; set; }

        public DateTime GenerateAt { get; set; }

        public Guid CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(Customer.Recommendations))]
        public virtual Customer? Customer { get; set; }
    }
}
