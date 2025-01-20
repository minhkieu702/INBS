using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Infrastructure.SignalR
{
    public class NotificationHubService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IConnectionMapping _connectionMapping;

        public NotificationHubService(IHubContext<NotificationHub> hubContext, IConnectionMapping connectionMapping)
        {
            _hubContext = hubContext;
            _connectionMapping = connectionMapping;
        }

        //public async Task SendNotification()
    }
}
