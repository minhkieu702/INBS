using INBS.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Data.Models.Entities
{
    public class Admin
    {
        public Admin()
        {
            ID = Guid.NewGuid();
            AdminLogs = [];
        }

        [Key]
        public Guid ID { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(User.Admin))]
        public virtual User? User { get; set; }

        [InverseProperty(nameof(AdminLog.Admin))]
        public virtual ICollection<AdminLog> AdminLogs { get; set; }

    }
}
