using INBS.Application.DTOs.Common.Preference;
using INBS.Application.DTOs.Design.CustomDesign;
using INBS.Application.DTOs.Service.CustomCombo;
using INBS.Application.DTOs.User.User;
using System.ComponentModel.DataAnnotations;

namespace INBS.Application.DTOs.User.Customer
{
    public class CustomerResponse
    {
        [Key]
        public Guid ID { get; set; }

        public string? Preferences { get; set; }

        public UserResponse? User { get; set; }

        public ICollection<CustomDesignResponse> CustomDesigns { get; set; } = [];

        public ICollection<CustomComboResponse> CustomCombos { get; set; } = [];

        public ICollection<PreferenceResponse> CustomerPreferences { get; set; } = [];

        public ICollection<DeviceTokenResponse> DeviceTokens { get; set; } = [];
    }
}
