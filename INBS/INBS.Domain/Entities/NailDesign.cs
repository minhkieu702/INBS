using INBS.Domain.Common;
using INBS.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INBS.Domain.Entities
{
    public class NailDesign
    {
        public NailDesign()
        {
            ID = Guid.NewGuid();
            ImageUrl = Constants.DEFAULT_IMAGE_URL;
            IsLeft = false;
            NailDesignServices = [];
        }

        [Key]
        public Guid ID { get; set; }

        public Guid DesignId { get; set; }
        [ForeignKey(nameof(DesignId))]
        [InverseProperty(nameof(Design.NailDesigns))]
        public virtual Design? Design { get; set; }

        public string ImageUrl { get; set; }

        public int NailPosition { get; set; } //4, 8, 12, 16, 20

        public bool IsLeft { get; set; } //true - left, false - right

        [InverseProperty(nameof(NailDesignService.NailDesign))]
        public virtual ICollection<NailDesignService> NailDesignServices { get; set; }
    }
}
