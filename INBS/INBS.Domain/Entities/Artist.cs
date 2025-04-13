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
            Username = string.Empty;
            ArtistServices = [];
            ArtistStores = [];
        }
        [Key]
        public Guid ID { get; set; }
        [ForeignKey(nameof(ID))]
        [InverseProperty(nameof(User.Artist))]
        public virtual User? User { get; set; }

        public string Username { get; set; }

        public int YearsOfExperience { get; set; }

        public int Level { get; set; }

        public float AverageRating { get; set; }
        public virtual ICollection<ArtistCertificate> Certificates { get; set; } = [];


        [InverseProperty(nameof(ArtistService.Artist))]
        public virtual ICollection<ArtistService> ArtistServices { get; set; }

        [InverseProperty(nameof(ArtistStore.Artist))]
        public virtual ICollection<ArtistStore> ArtistStores { get; set; }
    }
}
