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
            Preferences = [];
            Description = string.Empty;
            CustomerSelecteds = [];
            Carts = [];
            Feedbacks = [];
        }
        [Key]
        public Guid ID { get; set; }
        [ForeignKey(nameof(ID))]
        [InverseProperty(nameof(User.Customer))]
        public virtual User? User { get; set; }

        public string? OtpCode { get; set; }

        public DateTime? OtpExpiry { get; set; }

        public bool IsVerified { get; set; }

        public string? Description { get; set; }

        [InverseProperty(nameof(Recommendation.Customer))]
        public virtual ICollection<Recommendation> Recommendations { get; set; }

        [InverseProperty(nameof(CustomerSelected.Customer))]
        public virtual ICollection<CustomerSelected> CustomerSelecteds { get; set; }

        [InverseProperty(nameof(Preference.Customer))]
        public virtual ICollection<Preference> Preferences { get; set; }

        [InverseProperty(nameof(Cart.Customer))]
        public virtual ICollection<Cart> Carts { get; set; }

        [InverseProperty(nameof(Feedback.Customer))]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
