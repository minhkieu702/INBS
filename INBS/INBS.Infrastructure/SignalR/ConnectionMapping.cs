using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Infrastructure.SignalR
{
    public interface IConnectionMapping
    {
        void Add(string connectionId, Guid userId);
        void Remove(string connectionId);
        IEnumerable<Guid> GetConnections(Guid userId);
        Guid? GetUserId(string connectionId);
        IReadOnlyDictionary<string, Guid> GetAllConnections();
    }
    public class ConnectionMapping : IConnectionMapping
    {
        private readonly Dictionary<string, Guid> _connections = new Dictionary<string, Guid>();

        public void Add(string connectionId, Guid userId)
        {
            _connections.TryAdd(connectionId, userId);
        }

        public IReadOnlyDictionary<string, Guid> GetAllConnections()
        {
            return _connections;
        }

        public IEnumerable<Guid> GetConnections(Guid userId)
        {
            return _connections
                .Where(kvp => kvp.Value == userId)
                .Select(kvp => kvp.Value);
        }

        public Guid? GetUserId(string connectionId)
        {
            return _connections.TryGetValue(connectionId, out var userId) ? userId : null;
        }

        public void Remove(string connectionId)
        {
            _connections.Remove(connectionId);
        }
    }
}
