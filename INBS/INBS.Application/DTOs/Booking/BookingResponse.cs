using INBS.Application.DTOs.ArtistStore;
using INBS.Application.DTOs.Common;
using INBS.Application.DTOs.CustomerSelected;
using INBS.Application.DTOs.Feedback;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Booking
{
    public class BookingResponse : BaseEntity
    {
        public BookingResponse() : base() { }

        public DateOnly ServiceDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly PredictEndTime { get; set; }

        public string Status { get; set; } = string.Empty;

        public long TotalAmount { get; set; }

        public Guid CustomerSelectedId { get; set; }

        public virtual CustomerSelectedResponse? CustomerSelected { get; set; }

        public Guid ArtistStoreId { get; set; }
        
        public virtual ArtistStoreResponse? ArtistStore { get; set; }

        //public virtual ICollection<Cancellation> Cancellations { get; set; } = [];

        public virtual ICollection<FeedbackResponse> Feedbacks { get; set; } = [];
    }
}
