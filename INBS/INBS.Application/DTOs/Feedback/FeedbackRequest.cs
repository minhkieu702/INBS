using INBS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Feedback
{
    public class FeedbackRequest
    {
        public FeedbackType FeedbackType { get; set; }

        public int Rating { get; set; }

        public Guid Content { get; set; }

        public Guid BookingId { get; set; }
    }
}
