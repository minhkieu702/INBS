using INBS.Application.DTOs.Common;
using INBS.Application.DTOs.Common.Preference;
using INBS.Application.DTOs.Design.DesignService;
using INBS.Application.DTOs.Design.Image;
using INBS.Application.DTOs.Design.NailDesign;

namespace INBS.Application.DTOs.Design.Design
{
    public class DesignResponse : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public float TrendScore { get; set; }

        public string? Description { get; set; }

        public int Price { get; set; }

        public int AverageRating { get; set; }

        public ICollection<ImageResponse> Images { get; set; } = [];

        public ICollection<PreferenceResponse> Preferences { get; set; } = [];

        public ICollection<NailDesignResponse> NailDesigns { get; set; } = [];

        public ICollection<DesignServiceResponse> DesignServices { get; set; } = [];
    }
}
