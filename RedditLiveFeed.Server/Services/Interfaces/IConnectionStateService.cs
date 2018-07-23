using System.Collections.Generic;
using RedditLiveFeed.Server.Model;

namespace RedditLiveFeed.Server.Services.Interfaces
{
    public interface IConnectionStateService
    {
        List<HubConnectionState> GetAll();
        void AddConnection(HubConnectionState state);
        void RemoveConnection(string id);
    }
}
