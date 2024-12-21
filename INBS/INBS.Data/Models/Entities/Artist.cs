using INBS.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Data.Models.Entities
{
    public class Artist
    {
        public Artist()
        {
            ID = Guid.NewGuid();
            WaitLists = [];
            Bookings = [];
            ArtistAvailabilities = [];
        }

        [Key]
        public Guid ID { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(User.Artist))]
        public virtual User? User { get; set; }

        [InverseProperty(nameof(ArtistAvailability.Artist))]
        public virtual ICollection<ArtistAvailability> ArtistAvailabilities { get; set; }

        [InverseProperty(nameof(WaitList.Artist))]
        public virtual ICollection<WaitList> WaitLists { get; set; }

        [InverseProperty(nameof(Booking.Artist))]
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
