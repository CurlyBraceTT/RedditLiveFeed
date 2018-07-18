using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedditLiveFeed.Model;
using RedditLiveFeed.Services.Interfaces;
using RedditLiveFeed.Utils;
using RedditLiveFeed.Utils.Interfaces;

namespace RedditLiveFeed.Services
{
    public class RedditApiService : IRedditApiService
    {
        private readonly IRedditClientFactory _clientFactory;
        public const string FEED_URL = "https://oauth.reddit.com/r/mgtow/new";

        public RedditApiService(IRedditClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<RedditListing> GetNew(string before = "", string after = "", int limit = 0)
        {
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

            var url = QueryHelpers.AddQueryString(FEED_URL, parameters);

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            JObject parsed = JObject.Parse(content);
            var feed = JsonConvert.DeserializeObject<RedditListing>(parsed["data"].ToString());
            return feed;
        }
    }
}
