using INBS.Application.DTOs.Design.Image;
using INBS.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Design.Design
{
    public class DesignRequest
    {
        public string Name { get; set; } = string.Empty;

        public float TrendScore { get; set; }

        public string? Description { get; set; }

        public int Price { get; set; }

        public IList<int> ColorIds { get; set; } = [];

        public IList<int> OccasionIds { get; set; } = [];

        public IList<int> SkintoneIds { get; set; } = [];

        public IList<int> PaintTypeIds { get; set; } = [];
    }
}
