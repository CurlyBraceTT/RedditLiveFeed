using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RedditLiveFeed.Model;
using RedditLiveFeed.Services.Interfaces;

namespace RedditLiveFeed.Services
{
    public class RedditFeedService : IRedditFeedService
    {
        private readonly ConcurrentDictionary<string, RedditFeed> _data = 
            new ConcurrentDictionary<string, RedditFeed>();

        public RedditFeedService() { }

        public void AddFeed(string id, RedditFeed feed)
        {
            _data.TryAdd(id, feed);
        }

        public List<RedditFeed> GetAll()
        {
            return _data.Values.ToList();
        }

        public RedditFeed GetFeed(string id)
        {
            return _data[id];
        }

        public bool TryGetFeed(string id, out RedditFeed feed)
        {
            return _data.TryGetValue(id, out feed);
        }

        public bool RemoveFeed(string id)
        {
            return _data.TryRemove(id, out var feed);
        }
    }
}
