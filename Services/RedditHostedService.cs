using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using RedditLiveFeed.Model;
using RedditLiveFeed.Services.Interfaces;

namespace RedditLiveFeed.Services
{
    public class RedditHostedService : MyHostedService
    {
        private readonly IMemoryCache _cache;
        private readonly IRedditFeedService _feedService;

        public INotifyService Service;

        public RedditHostedService(IMemoryCache cache, 
            INotifyService service,
            IRedditFeedService feedService)
        {
            _cache = cache;
            Service = service;
            _feedService = feedService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var key = "new.rss";

                try
                {
                    var feed = await _feedService.GetNew();

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(3));

                    if(_cache.TryGetValue(key + feed.Updated, out RedditFeed cachedFeed))
                    {
                        if(cachedFeed.Updated > feed.Updated)
                        {
                            feed = cachedFeed;
                        }
                    }

                    _cache.Set(key + feed.Updated, feed, cacheEntryOptions);
                    await Service.NotifyAsync(feed);
                }
                catch(Exception ex)
                {
                    Debug.Write(ex.Message);
                }

                await Task.Delay(500, stoppingToken);
            }
            while (!stoppingToken.IsCancellationRequested);
        }
    }
}
