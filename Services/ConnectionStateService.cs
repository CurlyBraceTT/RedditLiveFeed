using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RedditLiveFeed.Model;
using RedditLiveFeed.Services.Interfaces;

namespace RedditLiveFeed.Services
{
    public class ConnectionStateService : IConnectionStateService
    {
        private readonly ConcurrentDictionary<string, HubConnectionState> _data = 
            new ConcurrentDictionary<string, HubConnectionState>();

        public ConnectionStateService() { }

        public void AddConnection(HubConnectionState state)
        {
            _data.TryAdd(state.ConnectionId, state);
        }

        public List<HubConnectionState> GetAll()
        {
            return _data.Values.ToList();
        }

        public void RemoveConnection(string id)
        {
            _data.TryRemove(id, out var connection);
        }
    }
}
