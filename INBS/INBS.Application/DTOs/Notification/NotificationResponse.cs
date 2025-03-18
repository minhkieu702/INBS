﻿using INBS.Application.DTOs.Common;
using INBS.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Notification
{
    public class NotificationResponse : BaseEntity
    {
        public int Status { get; set; } //Sent, Read, Removed

        public int NotificationType { get; set; } //Reminder, Promotion, Alert

        public Guid UserId { get; set; }
        public virtual UserResponse? User { get; set; }
    }
}
