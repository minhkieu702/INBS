using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service.TemplateCombo
{
    public class ServiceTemplateComboRequest
    {
        public Guid ServiceId { get; set; }
        public int NumerialOrder { get; set; }
    }
}
