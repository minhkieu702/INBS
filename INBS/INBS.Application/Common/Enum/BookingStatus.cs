using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Common.Enum
{
    public enum BookingStatus
    {
        isWating = 0,
        isBooked = 1,
        isCompleted = 2,
        isCancelled = -1
    }
}
