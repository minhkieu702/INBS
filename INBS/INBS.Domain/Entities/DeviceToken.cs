using INBS.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class DeviceToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Token { get; set; } = string.Empty;

        public int Platform { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(User.DeviceTokens))]
        public virtual User? User{ get; set; }
    }
}
