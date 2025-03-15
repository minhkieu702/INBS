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
    public class Notification : BaseEntity
    {
        public Notification() : base()
        {
            
        }
        public int Status { get; set; } //Sent, Read, Removed

        public int NotificationType { get; set; } //Reminder, Promotion, Alert

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(User.Notifications))]
        public virtual User? User { get; set; }
    }
}
