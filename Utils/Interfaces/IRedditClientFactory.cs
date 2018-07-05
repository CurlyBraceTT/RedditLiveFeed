using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RedditLiveFeed.Utils.Interfaces
{
    // Created RedditClient, Stores a state in cache
    // Lock creating method when implementing
    public interface IRedditClientFactory
    {
        Task<IRedditClient> GetClient();
    }
}
