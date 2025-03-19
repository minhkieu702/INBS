using INBS.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Entities
{
    public class ServicePriceHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public Guid ServiceId { get; set; }
        [ForeignKey(nameof(ServiceId))]
        [InverseProperty(nameof(Service.ServicePriceHistories))]
        public virtual Service? Service { get; set; }

        public long Price { get; set; } // Giá tại thời điểm này

        public DateTime EffectiveFrom { get; set; } // Ngày bắt đầu áp dụng

        public DateTime? EffectiveTo { get; set; } // Ngày kết thúc (null nếu là giá hiện tại)
    }

}
