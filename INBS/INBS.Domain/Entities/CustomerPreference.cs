using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class CustomerPreference
    {
        public CustomerPreference()
        {
            PreferenceType = string.Empty;
        }

        public Guid CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(Customer.CustomerPreferences))]
        public virtual Customer? Customer { get; set; }

        public Guid PreferenceId { get; set; }

        public string PreferenceType { get; set; }
    }
}
