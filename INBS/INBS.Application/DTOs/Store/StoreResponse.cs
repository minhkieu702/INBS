

using INBS.Application.DTOs.Common;

namespace INBS.Application.DTOs.Store
{
    public class StoreResponse : BaseEntity
    {
        public string Address { get; set; }

        public string? Description { get; set; }

        public string ImageUrl { get; set; }

        public string Status { get; set; } // 0: inactive, 1: active

#warning Add ArtistResponse, AdminResponse, DesignResponse

        public ICollection<StoreServiceResponse> StoreServices { get; set; } = [];

        public ICollection<StoreDesignResponse> StoreDesigns { get; set; }

    }
}
