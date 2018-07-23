using System.Collections.Generic;
using RedditLiveFeed.Main.Model;

namespace RedditLiveFeed.Server.Services.Interfaces
{
    public interface IRedditFeedService
    {
        List<RedditFeed> GetAll();
        void AddFeed(string id, RedditFeed feed);
        RedditFeed GetFeed(string id);
        bool TryGetFeed(string id, out RedditFeed feed);
        bool RemoveFeed(string id);
    }
}
