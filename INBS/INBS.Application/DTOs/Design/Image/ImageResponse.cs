using INBS.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Design.Image
{
    public class ImageResponse
    {
        [Key]
        public Guid ID { get; set; }

        public int NumerialOrder { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
