using INBS.Domain.Entities;
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
    public class Customer
    {
        public Customer()
        {
            Recommendations = [];
            CustomCombos = [];
            CustomDesigns = [];
            CustomerPreferences = [];
            DeviceTokens = [];
        }
        [Key]
        public Guid ID { get; set; }
        [ForeignKey(nameof(ID))]
        [InverseProperty(nameof(User.Customer))]
        public virtual User? User { get; set; }

        [InverseProperty(nameof(Recommendation.Customer))]
        public virtual ICollection<Recommendation> Recommendations { get; set; }

        [InverseProperty(nameof(CustomCombo.Customer))]
        public virtual ICollection<CustomCombo> CustomCombos { get; set; }

        [InverseProperty(nameof(CustomDesign.Customer))]
        public virtual ICollection<CustomDesign> CustomDesigns { get; set; }

        [InverseProperty(nameof(CustomerPreference.Customer))]
        public virtual ICollection<CustomerPreference> CustomerPreferences { get; set; }
            
        [InverseProperty(nameof(DeviceToken.Customer))]
        public virtual ICollection<DeviceToken> DeviceTokens { get; set; }
    }
}
