using INBS.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Data.Models.Entities
{
    public class Occasion
    {
        public Occasion()
        {
            ID = Guid.NewGuid();
            Name = string.Empty;
            NailDesignOccasions = [];
            OccasionPreferences = [];
        }

        [Key]
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        [InverseProperty(nameof(NailDesignOccasion.Occasion))]
        public virtual ICollection<NailDesignOccasion> NailDesignOccasions { get; set; }

        [InverseProperty(nameof(OccasionPreference.Occasion))]
        public virtual ICollection<OccasionPreference> OccasionPreferences { get; set; }
    }
}
