using INBS.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace INBS.Domain.Entities
{
    public class Design : BaseEntity
    {
        public Design() : base()
        {
            Name = string.Empty;
            Medias = [];
            NailDesigns = [];
            Preferences = [];
        }

        public string Name { get; set; }

        public float TrendScore { get; set; }

        public string? Description { get; set; }

        public float AverageRating { get; set; }

        [InverseProperty(nameof(Media.Design))]
        public virtual ICollection<Media> Medias { get; set; }

        [InverseProperty(nameof(NailDesign.Design))]
        public virtual ICollection<NailDesign> NailDesigns { get; set; }

        [InverseProperty(nameof(Preference.Design))]
        public virtual ICollection<Preference> Preferences { get; set; }
    }
}
