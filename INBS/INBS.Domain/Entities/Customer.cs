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
    public class Customer
    {
        public Customer()
        {
            ID = Guid.NewGuid();
            Recommendations = [];
            CustomCombos = [];
            CustomDesigns = [];
            CustomerPreferences = [];
        }

        [Key]
        public Guid ID { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
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
    }
}
