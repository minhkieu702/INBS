using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class Notification
    {
        public Notification()
        {
            ID = Guid.NewGuid();
        }

        [Key]
        public Guid ID { get; set; }

        public int Status { get; set; } //Sent, Read, Removed

        public DateTime NotifyAt { get; set; }

        public int Type { get; set; } //Reminder, Promotion, Alert

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(User.Notifications))]
        public virtual User? User { get; set; }
    }
}
