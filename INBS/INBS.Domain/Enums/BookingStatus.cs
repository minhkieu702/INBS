using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Domain.Enums
{
    public enum BookingStatus
    {
        isCanceled = -1,
        isWating = 0,
        isBooked = 1,
        isCompleted = 2
    }
}
