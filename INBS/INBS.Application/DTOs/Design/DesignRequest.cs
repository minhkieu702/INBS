using INBS.Application.DTOs.NailDesign;

namespace INBS.Application.DTOs.Design
{
    public class DesignRequest
    {
        public string Name { get; set; } = string.Empty;

        public float TrendScore { get; set; }

        public string? Description { get; set; }

        public int Price { get; set; }
    }
}
