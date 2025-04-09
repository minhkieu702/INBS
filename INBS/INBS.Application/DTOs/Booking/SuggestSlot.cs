using INBS.Application.DTOs.Artist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Booking
{
    public class SuggestSlot
    {
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
    }
}
