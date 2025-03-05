

using INBS.Application.DTOs.Common;
using INBS.Application.DTOs.User.Artist;
using INBS.Domain.Common;

namespace INBS.Application.DTOs.Store
{
    public class StoreResponse : BaseEntity
    {
        public string Address { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string ImageUrl { get; set; } = Constants.DEFAULT_IMAGE_URL;

        public string Status { get; set; } = "active";// 0: inactive, 1: active

#warning Add AdminResponse
        public ArtistResponse? Artist { get; set; }

        public ICollection<ArtistServiceResponse> ArtistServices { get; set; } = [];

        public ICollection<ArtistDesignResponse> ArtistDesigns { get; set; } = [];

    }
}
