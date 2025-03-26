using INBS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.DTOs.DeviceToken
{
    public class DeviceTokenRequest
    {
        public DevicePlatformType PlatformType { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
