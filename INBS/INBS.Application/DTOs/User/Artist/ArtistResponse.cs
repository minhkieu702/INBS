using INBS.Application.DTOs.Common;
using INBS.Application.DTOs.Store;
using INBS.Application.DTOs.User.Artist.ArtistAvailability;
using INBS.Application.DTOs.User.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.User.Artist
{
    public class ArtistResponse
    {
        public Guid ID { get; set; }

        public virtual UserResponse? User { get; set; }

        public int YearsOfExperience { get; set; }

        public int Level { get; set; }

        public int AverageRating { get; set; }

        public Guid StoreId { get; set; }

        public virtual StoreResponse? Store { get; set; }

        public virtual ICollection<ArtistDesignResponse> ArtistDesigns { get; set; } = [];

        public virtual ICollection<ArtistServiceResponse> ArtistServices { get; set; } = [];

        public virtual ICollection<ArtistAvailabilityResponse> ArtistAvailabilitys { get; set; } = [];
    }
}
