using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INBS.Domain.Entities.Common;

namespace INBS.Domain.Entities
{
    public class Artist : BaseEntity
    {
        public Artist() : base()
        {
            ArtistAvailabilities = [];
        }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(User.Artist))]
        public virtual User? User { get; set; }

        public Guid StoreID { get; set; }
        [InverseProperty(nameof(Store.Artists))]
        public virtual Store? Store { get; set; }

        [InverseProperty(nameof(ArtistAvailability.Artist))]
        public virtual ICollection<ArtistAvailability> ArtistAvailabilities { get; set; }
    }
}
