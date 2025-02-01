using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Design.CustomNailDesign
{
    public class AccessoryCustomNailDesignRequest
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Guid AccessoryId { get; set; }
    }
}
