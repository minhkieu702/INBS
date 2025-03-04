using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.Common.Preference
{
    public class PreferenceRequest
    {
        public IList<int> ColorIds { get; set; } = [];

        public IList<int> OccasionIds { get; set; } = [];

        public IList<int> SkintoneIds { get; set; } = [];

        public IList<int> PaintTypeIds { get; set; } = [];
    }
}
