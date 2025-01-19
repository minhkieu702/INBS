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
    public class CustomCombo : BaseEntity
    {
        public CustomCombo() : base()
        {
            IsFavorite = false;
            Bookings = [];
            ServiceCustomCombos = [];
        }
        public bool IsFavorite { get; set; }

        public Guid CustomerID { get; set; }
        [ForeignKey(nameof(CustomerID))]
        [InverseProperty(nameof(Customer.CustomCombos))]
        public virtual Customer? Customer { get; set; }

        [InverseProperty(nameof(ServiceCustomCombo.CustomCombo))]
        public virtual ICollection<ServiceCustomCombo> ServiceCustomCombos { get; set; }

        [InverseProperty(nameof(Booking.CustomCombo))]
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
