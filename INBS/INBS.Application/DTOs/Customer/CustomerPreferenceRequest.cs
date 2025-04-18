using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Customer
{
    public class CustomerPreferenceRequest
    {
        public IList<int> OccasionIds { get; set; } = [];

        public IList<int> SkintoneIds { get; set; } = [];
    }
}
