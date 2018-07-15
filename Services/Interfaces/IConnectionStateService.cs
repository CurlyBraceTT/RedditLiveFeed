using System.Collections.Generic;
using System.Data;
using RedditLiveFeed.Model;

namespace RedditLiveFeed.Services.Interfaces
{
    public interface IConnectionStateService
    {
        List<HubConnectionState> GetAll();
        void AddConnection(HubConnectionState state);
        void RemoveConnection(string id);
    }
}
