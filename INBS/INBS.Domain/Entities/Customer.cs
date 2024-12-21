using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class Customer
    {
        public Customer()
        {
            ID = Guid.NewGuid();
            FavoriteDesigns = [];
            OccasionPreferences = [];
            WaitLists = [];
            Bookings = [];
            Recommendations = [];
        }

        [Key]
        public Guid ID { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(User.Customer))]
        public virtual User? User { get; set; }

        [InverseProperty(nameof(WaitList.Customer))]
        public virtual ICollection<WaitList> WaitLists { get; set; }

        [InverseProperty(nameof(Booking.Customer))]
        public virtual ICollection<Booking> Bookings { get; set; }

        [InverseProperty(nameof(FavoriteDesign.Customer))]
        public virtual ICollection<FavoriteDesign> FavoriteDesigns { get; set; }

        [InverseProperty(nameof(SkinTone.Customer))]
        public virtual SkinTone? SkinTone { get; set; }

        [InverseProperty(nameof(OccasionPreference.Customer))]
        public virtual ICollection<OccasionPreference> OccasionPreferences { get; set; }

        [InverseProperty(nameof(Recommendation.Customer))]
        public virtual ICollection<Recommendation> Recommendations { get; set; }
    }
}
