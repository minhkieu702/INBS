using INBS.Application.DTOs.Artist;
using INBS.Application.DTOs.Common;
using INBS.Application.DTOs.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.ArtistStore
{
    public class ArtistStoreResponse : BaseEntity
    {
        public Guid ArtistId { get; set; }

        public virtual ArtistResponse? Artist { get; set; }

        public Guid StoreId { get; set; }

        public virtual StoreResponse? Store { get; set; }

        public DateOnly WorkingDate { get; set; }

        public long BreakTime { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

#warning response booking
    }
}
