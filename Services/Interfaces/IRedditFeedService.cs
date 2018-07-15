using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RedditLiveFeed.Model;

namespace RedditLiveFeed.Services.Interfaces
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
