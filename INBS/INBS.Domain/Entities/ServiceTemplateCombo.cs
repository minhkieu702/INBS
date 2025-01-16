using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class ServiceTemplateCombo
    {
        public Guid TemplateComboId { get; set; }
        [ForeignKey(nameof(TemplateComboId))]
        [InverseProperty(nameof(TemplateCombo.ServiceTemplateCombos))]
        public virtual TemplateCombo? TemplateCombo { get; set; }

        public Guid ServiceId { get; set; }
        [ForeignKey(nameof(ServiceId))]
        [InverseProperty(nameof(Service.ServiceTemplateCombos))]
        public virtual Service? Service { get; set; }
    }
}
