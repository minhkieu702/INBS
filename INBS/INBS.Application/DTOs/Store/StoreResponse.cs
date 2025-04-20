

using INBS.Application.DTOs.Artist;
using INBS.Application.DTOs.ArtistService;
using INBS.Application.DTOs.ArtistStore;
using INBS.Application.DTOs.Common;
using INBS.Domain.Common;

namespace INBS.Application.DTOs.Store
{
    public class StoreResponse : BaseEntity
    {
        public string Province { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string ImageUrl { get; set; } = Constants.DEFAULT_IMAGE_URL;

        public int Status { get; set; }

        public float Latitude { get; set; }

        public float Longtitude { get; set; }

        public float AverageRating { get; set; }

        public virtual ICollection<ArtistStoreResponse> ArtistStores { get; set; } = [];
    }
}
