using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSBS.Data.Models.Entities
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
            UserWaitLists = [];
            ArtistAvailabilities = [];
            AdminLogs = [];
            UserBookings = [];
            Recommendations = [];
        }

        [Key]
        public Guid ID { get; set; }

        public string? FullName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string? PhoneNumber { get; set; }

        public int Role { get; set; } = 0;

        public string? Preferences { get; set; }

        public DateTime CreateAt { get; set; }

        [InverseProperty(nameof(UserWaitList.User))]
        public virtual ICollection<UserWaitList> UserWaitLists { get; set; }

        //[InverseProperty(nameof (CustomerPreference))]
        //public virtual ICollection<CustomerPreference> CustomerPreferences { get; set; }

        [InverseProperty(nameof(ArtistAvailability.Artist))]
        public virtual ICollection<ArtistAvailability> ArtistAvailabilities { get; set; }

        [InverseProperty(nameof(AdminLog.Admin))]
        public virtual ICollection<AdminLog> AdminLogs { get; set; }

        [InverseProperty(nameof(UserBooking.User))]
        public virtual ICollection<UserBooking> UserBookings { get; set; }

        [InverseProperty(nameof(Recommendation.Customer))]
        public virtual ICollection<Recommendation> Recommendations { get; set; }
    }
}
