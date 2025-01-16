using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class User
    {
        public User()
        {
            ID = Guid.NewGuid();
            PasswordHash = string.Empty;
            Preferences = string.Empty;
            Email = string.Empty;
            CreateAt = DateTime.Now;
            Notifications = [];
        }

        [Key]
        public Guid ID { get; set; }

        public string? FullName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string? PhoneNumber { get; set; }

        public int Role { get; set; } //Customer, Artist, Admin

        public string? Preferences { get; set; }

        public DateTime CreateAt { get; set; }

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
