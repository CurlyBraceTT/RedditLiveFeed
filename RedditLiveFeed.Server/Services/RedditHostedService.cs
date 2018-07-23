using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RedditLiveFeed.Server.Services.Interfaces;
using RedditLiveFeed.Main.Services.Interfaces;

namespace RedditLiveFeed.Server.Services
{
    public class RedditHostedService : BaseHostedService
    {
        private readonly IRedditApiService _apiService;
        private readonly IMemoryCache _cache;
        private readonly IConnectionStateService _stateService;
        private readonly IRedditFeedService _feedService;
        private readonly ILogger<RedditHostedService> _logger;

        public RedditHostedService(IMemoryCache cache, 
            IRedditApiService apiService,
            IRedditFeedService feedService,
            IConnectionStateService stateService,
            ILogger<RedditHostedService> logger)
        {
            _cache = cache;
            _apiService = apiService;
            _stateService = stateService;
            _feedService = feedService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                try
                {
                    foreach(var feed in _feedService.GetAll())
                    {
                        var listing = await _apiService.GetNew(limit: 10);
                        if (listing.Children.Count > 0)
                        {
                            _logger.LogInformation("Pushing new data: {0}",
                                string.Join(", ", listing.Children.Select(d => d.Title)));
                            feed.AddRange(listing.Children);
                        }
                    }

                    var connections = _stateService.GetAll();
                    foreach (var c in connections)
                    {
                        if (_feedService.TryGetFeed(c.FeedId, out var existingFeed))
                        {
                            var data = existingFeed.GetData(c.LastEntryName);
                            if(data.Count() > 0)
                            {
                                _logger.LogInformation("Streaming new data: {0}",
                                    string.Join(", ", data.Select(d => d.Title)));
                                _ = c.StreamChannel.Writer.WriteAsync(data, stoppingToken);
                                c.LastEntryName = existingFeed.LastEntryName;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.Message);
                }

                await Task.Delay(1000, stoppingToken);
            }
            while (!stoppingToken.IsCancellationRequested);
        }
    }
}
