using INBS.Application.Common.MyJsonConverters;
using INBS.Application.DTOs.Common;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.User.Artist.ArtistAvailability
{
    public class ArtistAvailabilityRequest
    {
        public Guid ArtistId { get; set; }

        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly AvailableDate { get; set; }

        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly StartTime { get; set; }

        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly EndTime { get; set; }

        public int BreakTime { get; set; } //minute

        public int MaximumBreakTime { get; set; }
    }
}
