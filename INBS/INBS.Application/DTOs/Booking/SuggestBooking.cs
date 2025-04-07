using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Booking
{
    public class SuggestBooking
    {
        public string StoreName { get; set; } = string.Empty;
        public DateOnly WorkingDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public long BreakTime { get; set; }
        public long TotalAmount { get; set; }
    }
}
