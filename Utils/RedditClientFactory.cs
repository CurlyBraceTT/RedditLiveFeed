using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using RedditLiveFeed.Utils.Interfaces;

namespace RedditLiveFeed.Utils
{
    public class RedditClientFactory : IRedditClientFactory
    {
        private readonly IServiceProvider _services;
        private readonly IMemoryCache _cache;
        private static readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);

        private const string CACHE_KEY = "__RedditClientFactory.RedditClient";

        public RedditClientFactory(IServiceProvider services)
        {
            _services = services;
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public async Task<IRedditClient> GetClient()
        {
            var client = _cache.Get<IRedditClient>(CACHE_KEY);

            if (client != null)
            {
                if (client.Expired)
                {
                    await client.RefreshAccess();
                }

                return client;
            }

            await _cacheLock.WaitAsync();
            try
            {
                client = _cache.Get<IRedditClient>(CACHE_KEY);

                if(client != null)
                {
                    if (client.Expired)
                    {
                        await client.RefreshAccess();
                    }

                    return client;
                }

                client = await CreateClient();
                _cache.Set(CACHE_KEY, client, new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpiration = null,
                        SlidingExpiration = null
                    });

                return client;
            }
            finally
            {
                _cacheLock.Release();
            }
        }

        protected virtual async Task<IRedditClient> CreateClient()
        {
            var client = _services.GetRequiredService<IRedditClient>();

            if (!client.Authenticated)
            {
                await client.Authenticate();
            }

            return client;
        }
    }
}
