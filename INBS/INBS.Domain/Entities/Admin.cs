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
        public string Username { get; set; } = string.Empty;

        [Key]
        public Guid ID { get; set; }
        [ForeignKey(nameof(ID))]
        [InverseProperty(nameof(User.Admin))]
        public virtual User? User { get; set; }
    }
}
