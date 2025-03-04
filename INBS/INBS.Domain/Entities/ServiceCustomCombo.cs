using System.ComponentModel.DataAnnotations.Schema;

namespace INBS.Domain.Entities
{
    public class ServiceCustomCombo
    {
        public Guid CustomComboId { get; set; }
        [ForeignKey(nameof(CustomComboId))]
        [InverseProperty(nameof(CustomCombo.ServiceCustomCombos))]
        public virtual CustomCombo? CustomCombo { get; set; }

        public Guid ServiceId { get; set; }
        [ForeignKey(nameof(ServiceId))]
        [InverseProperty(nameof(Service.ServiceCustomCombos))]
        public virtual Service? Service { get; set; }

        public long Duration { get; set; }
    }
}
