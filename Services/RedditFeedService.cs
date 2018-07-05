using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using RedditLiveFeed.Model;
using RedditLiveFeed.Services.Interfaces;
using RedditLiveFeed.Utils;
using RedditLiveFeed.Utils.Interfaces;

namespace RedditLiveFeed.Services
{
    public class RedditFeedService : IRedditFeedService
    {
        private readonly IRedditClientFactory _clientFactory;
        public const string FEED_URL = "https://oauth.reddit.com/new";

        public RedditFeedService(IRedditClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<RedditFeed> GetNew(string after = "")
        {
            var client = await _clientFactory.GetClient();

            var parameters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(after))
            {
                parameters.Add("after", after);
            }

            var url = QueryHelpers.AddQueryString(FEED_URL, parameters);

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            using(var response = await client.SendAsync(request))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    var feed = JsonConvert.DeserializeObject<RedditFeed>(content);
                    return feed;
                }
                catch (HttpRequestException ex) when (response.StatusCode == HttpStatusCode.Unauthorized)
                {


                }
            }

            return default(RedditFeed);
        }
    }
}
