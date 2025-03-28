using INBS.Application.Common.MyJsonConverters;
using INBS.Domain.Enums;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Booking
{
    public class BookingRequest
    {
        //[JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly ServiceDate { get; set; }
        //[JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly StartTime { get; set; }

        public Guid CustomerSelectedId { get; set; }

        public TimeOnly? PredictEndTime { get; set; }

        public Guid ArtistId { get; set; }

        public Guid StoreId { get; set; }
    }
}
