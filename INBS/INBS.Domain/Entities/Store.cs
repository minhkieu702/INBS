using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class Store
    {
        public Store()
        {
            ID = Guid.NewGuid();
            Artists = [];
            StoreDesigns = [];
            StoreServices = [];
            Address = string.Empty;
            ImageUrl = string.Empty;
        }

        [Key]
        public Guid ID { get; set; }

        public string Address { get; set; }

        public string? Description { get; set; }
        
        public string ImageUrl { get; set; }

        public int Status { get; set; }

        [InverseProperty(nameof(Artist.Store))]
        public virtual ICollection<Artist> Artists { get; set; }

        public Guid AdminId { get; set; }
        [ForeignKey(nameof(AdminId))]
        [InverseProperty(nameof(Admin.Stores))]
        public virtual Admin? Admin { get; set; }

        [InverseProperty(nameof(StoreDesign.Store))]
        public virtual ICollection<StoreDesign> StoreDesigns { get; set; }

        [InverseProperty(nameof(StoreService.Store))]
        public virtual ICollection<StoreService> StoreServices { get; set; }
    }
}
