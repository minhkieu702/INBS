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
    public class Admin : BaseEntity
    {
        public Admin() : base()
        {
            Stores = [];
        }
        public Guid UserId { get; set; }
        
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(User.Admin))]
        public virtual User? User { get; set; }

        [InverseProperty(nameof(Store.Admin))]
        public virtual ICollection<Store> Stores { get; set; }
    }
}
