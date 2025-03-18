using INBS.Application.DTOs.CustomerSelected;
using INBS.Application.DTOs.Feedback;
using INBS.Application.DTOs.Preference;
using INBS.Application.DTOs.User;
using System.ComponentModel.DataAnnotations;

namespace INBS.Application.DTOs.Customer
{
    public class CustomerResponse
    {
        [Key]
        public Guid ID { get; set; }

        public string? Description { get; set; }

        public virtual UserResponse? User { get; set; }

        public virtual ICollection<FeedbackResponse> Feedbacks { get; set; } = [];

        public virtual ICollection<CustomerSelectedResponse> CustomerSelecteds { get; set; } = [];

        public virtual ICollection<PreferenceResponse> Preferences { get; set; } = [];

        public virtual ICollection<DeviceTokenResponse> DeviceTokens { get; set; } = [];
    }
}
