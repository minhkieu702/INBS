using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.NailDesign
{
    public class NailDesignServiceRequest
    {
        public Guid ServiceId { get; set; }
        public long ExtraPrice { get; set; }
    }
}
