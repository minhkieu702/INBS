using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service.DesignService
{
    public class ServiceDesignRequest
    {
        public Guid DesignId { get; set; }

        public long ExtraPrice { get; set; }
    }
}
