using INBS.Application.DTOs.Common;
using INBS.Application.DTOs.DesignService;
using INBS.Application.DTOs.Image;
using INBS.Application.DTOs.NailDesign;
using INBS.Application.DTOs.Preference;

namespace INBS.Application.DTOs.Design
{
    public class DesignResponse : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public float TrendScore { get; set; }

        public string? Description { get; set; }

        public int Price { get; set; }

        public int AverageRating { get; set; }

        public ICollection<MediaResponse> Images { get; set; } = [];

        public ICollection<PreferenceResponse> Preferences { get; set; } = [];

        public ICollection<NailDesignResponse> NailDesigns { get; set; } = [];

        public ICollection<NailDesignServiceResponse> DesignServices { get; set; } = [];
    }
}
