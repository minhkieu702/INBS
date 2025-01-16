using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class StoreService
    {
        public Guid StoreId { get; set; }
        [ForeignKey(nameof(StoreId))]
        [InverseProperty(nameof(Store.StoreServices))]
        public virtual Store? Store { get; set; }

        public Guid ServiceId { get; set; }
        [ForeignKey(nameof(ServiceId))]
        [InverseProperty(nameof(Service.StoreServices))]
        public virtual Service? Service { get; set; }
    }
}
