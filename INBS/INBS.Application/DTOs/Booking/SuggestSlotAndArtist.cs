using INBS.Application.DTOs.Artist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Booking
{
    public class SuggestSlotAndArtist
    {
        public IList<Time> Times { get; set; } = new List<Time>();
        public IList<ArtistResponse> Artists { get; set; } = new List<ArtistResponse>();
    }

    public class Time
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
