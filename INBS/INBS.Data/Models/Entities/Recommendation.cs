using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Data.Models.Entities
{
    public class Recommendation
    {
        public Recommendation()
        {
            ID = Guid.NewGuid();
        }

        [Key]
        public Guid ID { get; set; }

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
