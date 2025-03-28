using INBS.Application.DTOs.DeviceToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.IServices
{
    public interface IDeviceTokenService
    {
        Task AddDeviceToken(DeviceTokenRequest deviceTokenRequest);
        IQueryable<DeviceTokenResponse> Get();
        Task RemoveDeviceToken(string deviceToken);
    }
}
