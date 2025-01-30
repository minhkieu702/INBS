using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service.CustomCombo
{
    public class ServiceCustomComboRequest
    {
        public Guid ServiceId { get; set; }
        public int NumerialOrder { get; set; }
    }
}
