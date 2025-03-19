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
        isWaiting = 0,
        isConfirmed = 1,
        isServing = 2,
        isCompleted = 3
    }
}
