﻿using INBS.Domain.Entities;
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
    public class User : BaseEntity
    {
        public User() : base()
        {
            PasswordHash = string.Empty;
            Notifications = [];
            ImageUrl = string.Empty;
            DeviceTokens = [];
            Email = string.Empty;
        }


        [InverseProperty(nameof(DeviceToken.User))]
        public virtual ICollection<DeviceToken> DeviceTokens { get; set; }

        public string? FullName { get; set; }

        public string PasswordHash { get; set; }

        public string? PhoneNumber { get; set; }

        public string Email { get; set; }

        public int Role { get; set; } //Customer, Artist, Admin

        public DateOnly DateOfBirth { get; set; }

        public string ImageUrl { get; set; }

        [InverseProperty(nameof(Customer.User))]
        public virtual Customer? Customer { get; set; }

        [InverseProperty(nameof(Admin.User))]
        public virtual Admin? Admin { get; set; }

        [InverseProperty(nameof(Artist.User))]
        public virtual Artist? Artist { get; set; }

        [InverseProperty(nameof(Notification.User))]
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
