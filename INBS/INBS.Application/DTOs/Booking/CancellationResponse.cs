using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Booking
{
    public class CancellationResponse
    {
        [Key]
        public int ID { get; set; }

        public string? Reason { get; set; }

        public DateTime CancelledAt { get; set; }

        public float Percent { get; set; }

        public Guid BookingId { get; set; }
    }
}
