using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.ArtistStore
{
    public class ArtistStoreRequest
    {
        public Guid StoreId { get; set; }

        public DateOnly WorkingDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }
    }
}
