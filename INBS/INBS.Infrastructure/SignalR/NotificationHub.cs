using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http.Connections.Features;

namespace INBS.Infrastructure.SignalR
{
    public class NotificationHub : Hub
    {
        private readonly IConnectionMapping _connectionMapping;

        public NotificationHub(IConnectionMapping connectionMapping)
        {
            _connectionMapping = connectionMapping;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.Features.Get<IHttpContextFeature>()?.HttpContext; // Update this line
            if (httpContext != null)
            {
                var accountIdStr = httpContext.Request.Query["userId"].ToString();

                if (string.IsNullOrEmpty(accountIdStr))
                {
                    throw new HubException("UserId is required");
                }

                var userId = Guid.Parse(accountIdStr);
                _connectionMapping.Add(Context.ConnectionId, userId);
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
                Console.WriteLine($"User {userId} connected");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _connectionMapping.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task RequestUnreadCount()
        {
            var accountId = _connectionMapping.GetUserId(Context.ConnectionId);
            if (accountId.HasValue)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{accountId}");
            }
        }
    }
}
