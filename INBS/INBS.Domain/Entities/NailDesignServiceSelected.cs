using System.ComponentModel.DataAnnotations.Schema;

namespace INBS.Domain.Entities
{
    public class NailDesignServiceSelected
    {
        public Guid CustomerSelectedId { get; set; }
        [ForeignKey(nameof(CustomerSelectedId))]
        [InverseProperty(nameof(CustomerSelected.NailDesignServiceSelecteds))]
        public virtual CustomerSelected? CustomerSelected { get; set; }

        public Guid NailDesignServiceId { get; set; }
        [ForeignKey(nameof(NailDesignServiceId))]
        [InverseProperty(nameof(NailDesignService.NailDesignServiceSelecteds))]
        public virtual NailDesignService? NailDesignService { get; set; }
    }
}
