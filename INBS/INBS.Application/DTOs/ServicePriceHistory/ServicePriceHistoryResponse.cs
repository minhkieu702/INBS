using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INBS.Application.DTOs.Service;

namespace INBS.Application.DTOs.ServicePriceHistory
{
    public class ServicePriceHistoryResponse
    {
            [Key]
            public int ID { get; set; }

            public Guid ServiceId { get; set; }
            public virtual ServiceResponse? Service { get; set; }

            public long Price { get; set; } // Giá tại thời điểm này

            public DateTime EffectiveFrom { get; set; } // Ngày bắt đầu áp dụng

            public DateTime? EffectiveTo { get; set; } // Ngày kết thúc (null nếu là giá hiện tại)
    }
}
