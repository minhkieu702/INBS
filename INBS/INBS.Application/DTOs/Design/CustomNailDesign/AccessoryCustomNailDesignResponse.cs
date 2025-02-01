using INBS.Application.DTOs.Design.Accessory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Design.CustomNailDesign
{
    public class AccessoryCustomNailDesignResponse
    {
        [Key]
        public Guid AccessoryId { get; set; }
        public virtual AccessoryResponse? Accessory { get; set; }
        [Key]
        public Guid CustomNailDesignId { get; set; }
        public virtual CustomNailDesignResponse? CustomNailDesign { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
