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
    public class ArtistStore : BaseEntity
    {
        public ArtistStore() : base()
        {
            Bookings = [];
        }
        public Guid ArtistId { get; set; }
        [ForeignKey(nameof(ArtistId))]
        [InverseProperty(nameof(Artist.ArtistStores))]
        public virtual Artist? Artist { get; set; }

        public Guid StoreId { get; set; }
        [ForeignKey(nameof(StoreId))]
        [InverseProperty(nameof(Store.ArtistStores))]
        public virtual Store? Store { get; set; }

        public DateOnly WorkingDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        [InverseProperty(nameof(Booking.ArtistStore))]
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
