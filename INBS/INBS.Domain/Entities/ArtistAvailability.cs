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
    public class ArtistAvailability
    {
        public ArtistAvailability()
        {
            ID = Guid.NewGuid();
        }

        [Key]
        public Guid ID { get; set; }
        
        public DateOnly AvailableDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public Guid ArtistId { get; set; }
        [ForeignKey(nameof(ArtistId))]
        [InverseProperty(nameof(Artist.ArtistAvailabilities))]
        public virtual Artist? Artist { get; set; }
    }
}
