using INBS.Domain.Entities.Common;
using INBS.Domain.Enums;
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
            Title = string.Empty;
            Status = (int)NotificationStatus.Send;
            Content = string.Empty;
            NotificationType = (int)Enums.NotificationType.Notification;
            WebHref = string.Empty;
            AppHref = string.Empty;
        }
        public int Status { get; set; } 

        public int NotificationType { get; set; } 

        public string Title { get; set; }

        public string Content { get; set; }

        public string WebHref { get; set; }

        public string AppHref { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(User.Notifications))]
        public virtual User? User { get; set; }
    }
}
