using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class CustomDesign
    {
        public CustomDesign()
        {
            ID = Guid.NewGuid();
            IsSave = false;
            ImageUrl = string.Empty;
            Bookings = [];
            AccessoryCustomDesigns = [];
        }

        [Key]
        public Guid ID { get; set; }

        public bool IsSave { get; set; }

        public string ImageUrl { get; set; }

        public Guid CustomerID { get; set; }
        [ForeignKey(nameof(CustomerID))]
        [InverseProperty(nameof(Customer.CustomDesigns))]
        public virtual Customer? Customer { get; set; }

        [InverseProperty(nameof(Booking.CustomDesign))]
        public virtual ICollection<Booking> Bookings { get; set; }

        [InverseProperty(nameof(AccessoryCustomDesign.CustomDesign))]
        public virtual ICollection<AccessoryCustomDesign> AccessoryCustomDesigns { get; set; }

        public Guid DesignID { get; set; }
        [ForeignKey(nameof(DesignID))]
        [InverseProperty(nameof(Design.CustomDesigns))]
        public virtual Design? Design { get; set; }
    }
}
