using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RedditLiveFeed.Main.Utils.Interfaces;

namespace RedditLiveFeed.Main.Utils
{
    public class RedditClientFactory : IRedditClientFactory
    {
        private readonly IServiceProvider _services;
        private readonly IMemoryCache _cache;
        private readonly ILogger<RedditClientFactory> _logger;
        private static readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);

        private const string CACHE_KEY = "__RedditClientFactory.RedditClient";

        public RedditClientFactory(IServiceProvider services, ILogger<RedditClientFactory> logger)
        {
            _services = services;
            _logger = logger;
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public async Task<IRedditClient> GetClient()
        {
            var client = await TryGetClient();

            if (client != null)
            {
                return client;
            }

            await _cacheLock.WaitAsync();
            try
            {
                client = await TryGetClient();

                if (client != null)
                {
                    return client;
                }

                _logger.LogInformation($"Creating new client...");
                client = await CreateClient();
                _cache.Set(CACHE_KEY, client, new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpiration = null,
                        SlidingExpiration = null
                    });

                _logger.LogInformation($"New client has been created");
                return client;
            }
            finally
            {
                _cacheLock.Release();
            }
        }

        protected async Task<IRedditClient> TryGetClient()
        {
            var client = _cache.Get<IRedditClient>(CACHE_KEY);

            if (client != null)
            {
                if (client.Expired)
                {
                    _logger.LogInformation($"Client access expired. Refreshing access...");
                    await client.RefreshAccess();
                    _logger.LogInformation($"Client access refreshed");
                }

                return client;
            }

            return null;
        }

        protected virtual async Task<IRedditClient> CreateClient()
        {
            var client = _services?.GetRequiredService<IRedditClient>();

            if (!client.Authenticated)
            {
                _logger.LogInformation($"Authenticating client...");
                await client.Authenticate();
                _logger.LogInformation($"Client authenticated successfully");
            }

            return client;
        }
    }
}
