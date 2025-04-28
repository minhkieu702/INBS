using INBS.Application.DTOs.Admin;
using INBS.Application.DTOs.Artist;
using INBS.Application.DTOs.Common;
using INBS.Application.DTOs.Customer;
using INBS.Application.DTOs.Notification;
using INBS.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.User
{
    public class UserResponse : BaseEntity
    {
        public string? FullName { get; set; }
        public string? Email {  get; set; }

        public string? PhoneNumber { get; set; }

        public int Role { get; set; } //Customer, Artist, Admin

        public DateOnly DateOfBirth { get; set; }

        public string ImageUrl { get; set; } = Constants.DEFAULT_IMAGE_URL;

        public CustomerResponse? Customer { get; set; }

        public AdminResponse? Admin { get; set; }

        public ArtistResponse? Artist { get; set; }

        public virtual ICollection<NotificationResponse> Notifications { get; set; } = [];
    }
}
