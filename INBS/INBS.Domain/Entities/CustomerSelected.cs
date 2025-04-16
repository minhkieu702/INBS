using INBS.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INBS.Domain.Entities
{
    public class CustomerSelected
    {
        public CustomerSelected()
        {
            ID = Guid.NewGuid();
            Bookings = [];
            NailDesignServiceSelecteds = [];
            IsDeleted = false;
        }

        [Key]
        public Guid ID { get; set; }

        public bool IsDeleted { get; set; }

        public Guid CustomerID { get; set; }
        [ForeignKey(nameof(CustomerID))]
        [InverseProperty(nameof(Customer.CustomerSelecteds))]
        public virtual Customer? Customer { get; set; }

        [InverseProperty(nameof(NailDesignServiceSelected.CustomerSelected))]
        public virtual ICollection<NailDesignServiceSelected> NailDesignServiceSelecteds { get; set; }

        [InverseProperty(nameof(Booking.CustomerSelected))]
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
