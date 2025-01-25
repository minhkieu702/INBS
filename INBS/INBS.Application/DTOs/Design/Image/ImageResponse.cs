using INBS.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Design.Image
{
    public class ImageResponse : BaseEntity
    {
        public int NumerialOrder { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public string? Description { get; set; }

        public Guid DesignId { get; set; }
    }
}
