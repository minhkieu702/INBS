using INBS.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INBS.Domain.Entities
{
    public class NailDesignService : BaseEntity
    {
        public NailDesignService() :base()
        {
            NailDesignServiceSelecteds = [];
            Carts = [];
        }
        public long AverageDuration { get; set; }

        public Guid NailDesignId { get; set; }
        [ForeignKey(nameof(NailDesignId))]
        [InverseProperty(nameof(NailDesign.NailDesignServices))]
        public virtual NailDesign? NailDesign { get; set; }

        public Guid ServiceId { get; set; }
        [ForeignKey(nameof(ServiceId))]
        [InverseProperty(nameof(Service.NailDesignServices))]
        public virtual Service? Service { get; set; }

        public long ExtraPrice { get; set; }

        [InverseProperty(nameof(NailDesignServiceSelected.NailDesignService))]
        public virtual ICollection<NailDesignServiceSelected> NailDesignServiceSelecteds { get; set; }

        [InverseProperty(nameof(Cart.NailDesignService))]
        public virtual ICollection<Cart> Carts { get; set; }
    }
}
