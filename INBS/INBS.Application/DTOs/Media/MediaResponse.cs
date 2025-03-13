using INBS.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Image
{
    public class MediaResponse
    {
        [Key]
        public int NumerialOrder { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        [Key]
        public Guid DesignId { get; set; }
        public int MediaType { get; set; }
    }
}
