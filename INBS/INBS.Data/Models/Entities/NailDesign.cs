using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSBS.Data.Models.Entities
{
    public class NailDesign
    {
        public NailDesign()
        {
            ID = Guid.NewGuid();
            Name = string.Empty;
            Images = [];
            Bookings = [];
            NailDesignOccasions = [];
        }

        [Key]
        public Guid ID { get; set; }

        public string Name { get; set; }

        public float TrendScore { get; set; }

        public DateTime CreatedAt { get; set; }

        [InverseProperty(nameof(Booking.Design))]
        public virtual ICollection<Booking> Bookings { get; set; }

        [InverseProperty(nameof(Image.Design))]
        public virtual ICollection<Image> Images { get; set; }

        [InverseProperty(nameof(NailDesignOccasion.Design))]
        public virtual ICollection<NailDesignOccasion> NailDesignOccasions { get; set; }


    }
}
