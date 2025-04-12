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
        private readonly static ConnectionMapping _connections = new();

        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid userGuid))
            {
                throw new HubException("UserId is required");
            }

            _connections.Add(Context.ConnectionId, userGuid);
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userGuid}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;
            var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid userGuid))
            {
                _connections.Remove(connectionId);
                await Groups.RemoveFromGroupAsync(connectionId, $"user_{userGuid}");
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task RequestUnreadCount()
        {
            var accountId = _connections.GetUserId(Context.ConnectionId);
            if (accountId.HasValue)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{accountId}");
            }
        }
    }
}
