using INBS.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Design.CustomNailDesign
{
    public class CustomNailDesignResponse
    {
        [Key]
        public Guid ID { get; set; }

        public Guid CustomDesignId { get; set; }

        public string ImageUrl { get; set; }

        public int NailPosition { get; set; }

        public bool IsLeft { get; set; }

        public virtual ICollection<AccessoryCustomNailDesignResponse> AccessoryCustomNailDesigns { get; set; } = [];
    }
}
