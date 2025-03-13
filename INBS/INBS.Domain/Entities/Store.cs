using INBS.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class Store : BaseEntity
    {
        public Store() : base()
        {
            Address = string.Empty;
            ImageUrl = string.Empty;
            ArtistStores = [];
        }

        public string Address { get; set; }

        public string? Description { get; set; }
        
        public string ImageUrl { get; set; }

        public int AverageRating { get; set; }

        public int Status { get; set; } // 0: inactive, 1: active

        [InverseProperty(nameof(ArtistStore.Store))]
        public virtual ICollection<ArtistStore> ArtistStores { get; set; }
    }
}
