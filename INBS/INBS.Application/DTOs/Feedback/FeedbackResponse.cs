using INBS.Application.DTOs.Booking;
using INBS.Application.DTOs.Common;
using INBS.Application.DTOs.Customer;
using INBS.Application.DTOs.FeedbackImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Feedback
{
    public class FeedbackResponse : BaseEntity
    {        
        public int FeedbackType { get; set; }

        public int Rating { get; set; }

        public string Content { get; set; } = string.Empty;

        public Guid TypeId { get; set; }

        public Guid? BookingId { get; set; }

        public Guid CustomerId { get; set; }

        public virtual CustomerResponse? Customer { get; set; }

        public virtual ICollection<FeedbackImageResponse> FeedbackImages { get; set; } = [];
    }
}
