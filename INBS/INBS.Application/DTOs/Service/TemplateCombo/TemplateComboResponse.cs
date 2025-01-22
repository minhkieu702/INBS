using INBS.Application.DTOs.Service.TemplateCombo;
using INBS.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Service.ServiceTemplateCombo
{
    public class TemplateComboResponse : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<ServiceTemplateComboResponse> ServiceTemplateCombos { get; set; } = [];
    }
}
