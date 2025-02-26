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
    public class Artist
    {
        public Artist()
        {
            ArtistServices = [];
            ArtistAvailabilities = [];
            ArtistDesigns = [];
        }
        [Key]
        public Guid ID { get; set; }
        [ForeignKey(nameof(ID))]
        [InverseProperty(nameof(User.Artist))]
        public virtual User? User { get; set; }

        public int YearsOfExperience { get; set; }

        public int Level { get; set; }

        public Guid StoreId { get; set; }
        [InverseProperty(nameof(Store.Artists))]
        public virtual Store? Store { get; set; }

        [InverseProperty(nameof(ArtistAvailability.Artist))]
        public virtual ICollection<ArtistAvailability> ArtistAvailabilities { get; set; }

        [InverseProperty(nameof(ArtistService.Artist))]
        public virtual ICollection<ArtistService> ArtistServices { get; set; }

        [InverseProperty(nameof(ArtistDesign.Artist))]
        public virtual ICollection<ArtistDesign> ArtistDesigns { get; set; }
    }
}
