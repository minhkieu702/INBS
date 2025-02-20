using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class ArtistService
    {
        public Guid ArtistId { get; set; }
        [ForeignKey(nameof(ArtistId))]
        [InverseProperty(nameof(Artist.ArtistServices))]
        public virtual Artist? Artist { get; set; }

        public Guid ServiceId { get; set; }
        [ForeignKey(nameof(ServiceId))]
        [InverseProperty(nameof(Service.ArtistServices))]
        public virtual Service? Service { get; set; }
    }
}
