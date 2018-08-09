using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedditLiveFeed.Main.Model;
using RedditLiveFeed.Main.Services.Interfaces;
using RedditLiveFeed.Main.Utils.Interfaces;

namespace RedditLiveFeed.Main.Services
{
    public class RedditApiService : IRedditApiService
    {
        private readonly IRedditClientFactory _clientFactory;
        private readonly ILogger<RedditApiService> _logger;
        public const string FEED_URL_TEMPLATE = "https://oauth.reddit.com/r/{0}/new";

        public RedditApiService(IRedditClientFactory clientFactory, ILogger<RedditApiService> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<RedditListing> GetNew(string subreddit, string before = "", string after = "", int limit = 0)
        {
            _logger.LogInformation($"Getting new posts for [{subreddit}]...");

            var url = string.Format(FEED_URL_TEMPLATE, subreddit);
            var client = await _clientFactory.GetClient();

            var parameters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(before))
            {
                parameters.Add("before", before);
            }

            if (!string.IsNullOrEmpty(after))
            {
                parameters.Add("after", after);
            }

            if(limit > 0)
            {
                parameters.Add("limit", limit.ToString());
            }

            var completeUrl = QueryHelpers.AddQueryString(url, parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, completeUrl);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            JObject parsed = JObject.Parse(content);
            var feed = JsonConvert.DeserializeObject<RedditListing>(parsed["data"].ToString());

            _logger.LogInformation($"Got [{feed.Children.Count}] new posts for [{subreddit}]");
            return feed;
        }
    }
}
