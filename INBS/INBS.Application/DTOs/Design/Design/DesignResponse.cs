using INBS.Application.DTOs.Common;
using INBS.Application.DTOs.Design.Image;
using INBS.Application.DTOs.Design.Preference;
using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Design.Design
{
    public class DesignResponse : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public float TrendScore { get; set; }

        public string? Description { get; set; }

        public int Price { get; set; }

        public ICollection<ImageResponse> Images { get; set; } = [];

        public ICollection<DesignPreferenceResponse> DesignPreferences { get; set; } = [];
    }
}
