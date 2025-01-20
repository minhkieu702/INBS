using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class DesignPreference
    {
        public DesignPreference()
        {
            PreferenceType = string.Empty;
        }

        public Guid DesignId { get; set; }
        [ForeignKey(nameof(DesignId))]
        [InverseProperty(nameof(Design.DesignPreferences))]
        public virtual Design? Design { get; set; }

        public int PreferenceId { get; set; }

        public string PreferenceType { get; set; }
    }
}
