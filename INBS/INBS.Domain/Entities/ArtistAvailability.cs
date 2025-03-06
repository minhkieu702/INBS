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
    public class ArtistAvailability : BaseEntity
    {
        public ArtistAvailability() : base()
        {
        }

        public DateOnly AvailableDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public long BreakTime { get; set; } //minute

        public long MaximumBreakTime { get; set; }

        public Guid ArtistId { get; set; }
        [ForeignKey(nameof(ArtistId))]
        [InverseProperty(nameof(Artist.ArtistAvailabilities))]
        public virtual Artist? Artist { get; set; }
    }
}
