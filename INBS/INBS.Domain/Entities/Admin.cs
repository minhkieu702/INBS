using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INBS.Domain.Entities.Common;

namespace INBS.Domain.Entities
{
    public class Admin
    {
        public Admin()
        {
            Stores = [];
        }
        [Key]
        public Guid ID { get; set; }
        [ForeignKey(nameof(ID))]
        [InverseProperty(nameof(User.Admin))]
        public virtual User? User { get; set; }

        [InverseProperty(nameof(Store.Admin))]
        public virtual ICollection<Store> Stores { get; set; }
    }
}
