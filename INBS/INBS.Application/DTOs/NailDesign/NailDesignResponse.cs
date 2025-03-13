using INBS.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.NailDesign
{
    public class NailDesignResponse
    {
        [Key]
        public Guid DesignId { get; set; }

        public string ImageUrl { get; set; } = Constants.DEFAULT_IMAGE_URL;

        [Key]
        public int NailPosition { get; set; } //4, 8, 12, 16, 20

        [Key]
        public bool IsLeft { get; set; } //true - left, false - right
    }
}
