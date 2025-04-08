using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service
{
    public class ServiceDurationRequest
    {
        public Guid ServiceId { get; set; }
        public TimeOnly StartAt { get; set; }
        public TimeOnly EndAt { get; set; }
    }
}
