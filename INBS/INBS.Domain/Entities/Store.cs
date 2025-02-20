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
            Artists = [];
            Address = string.Empty;
            ImageUrl = string.Empty;
        }

        public string Address { get; set; }

        public string? Description { get; set; }
        
        public string ImageUrl { get; set; }

        public int Status { get; set; } // 0: inactive, 1: active

        [InverseProperty(nameof(Artist.Store))]
        public virtual ICollection<Artist> Artists { get; set; }

        public Guid AdminId { get; set; }
        [ForeignKey(nameof(AdminId))]
        [InverseProperty(nameof(Admin.Stores))]
        public virtual Admin? Admin { get; set; }
    }
}
