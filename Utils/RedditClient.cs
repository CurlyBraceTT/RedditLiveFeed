using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RedditLiveFeed.Utils.Interfaces;

namespace RedditLiveFeed.Utils
{
    public class RedditClient : IRedditClient
    {
        public string RefreshToken { get; private set; }
        public string CurrentAccessToken { get; private set; }
        public DateTime ExpireTime { get; private set; }

        public const string ACCESS_URL = "https://www.reddit.com/api/v1/access_token";
        public const int SAFE_EXPRITED_GAP = 300;   // Refresh 5 minutes before expiration

        private readonly HttpClient _client;
        private readonly RedditAuthConfiguration _configuration;

        public bool Authenticated => !string.IsNullOrEmpty(CurrentAccessToken);
        public bool Expired => DateTime.Now > ExpireTime;

        public RedditClient(HttpClient client, IOptions<RedditAuthConfiguration> configuration)
        {
            _client = client;
            _configuration = configuration.Value;
        }

        public async Task Authenticate()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("username", _configuration.Username),
                new KeyValuePair<string, string>("password", _configuration.Password),
                new KeyValuePair<string, string>("duration", "permanent")
            });

            var authValue = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes(
                    $"{_configuration.ClientId}:{_configuration.Secret}")));

            var request = new HttpRequestMessage(HttpMethod.Post, ACCESS_URL)
            {
                Content = content
            };
            request.Headers.Authorization = authValue;

            var response = await _client.SendAsync(request);
            var authContent = await response.Content.ReadAsStringAsync();

            dynamic authDefinition = JsonConvert.DeserializeObject(authContent);
            CurrentAccessToken = authDefinition.access_token;
            int expiresIn = authDefinition.expires_in;
            ExpireTime = DateTime.Now.AddSeconds(expiresIn);
            if(expiresIn > SAFE_EXPRITED_GAP)
            {
                ExpireTime = ExpireTime.AddSeconds(- SAFE_EXPRITED_GAP);
            }

            RefreshToken = authDefinition.refresh_token;
        }

        public async Task RefreshAccess()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", RefreshToken)
            });

            var authValue = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes(
                    $"{_configuration.ClientId}:{_configuration.Secret}")));

            var request = new HttpRequestMessage(HttpMethod.Post, ACCESS_URL)
            {
                Content = content
            };
            request.Headers.Authorization = authValue;

            var response = await _client.SendAsync(request);
            var authContent = await response.Content.ReadAsStringAsync();

            dynamic authDefinition = JsonConvert.DeserializeObject(authContent);
            CurrentAccessToken = authDefinition.access_token;
            int expiresIn = authDefinition.expires_in;
            ExpireTime = DateTime.Now.AddSeconds(expiresIn);
            if (expiresIn > SAFE_EXPRITED_GAP)
            {
                ExpireTime = ExpireTime.AddSeconds(- SAFE_EXPRITED_GAP);
            }
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var authValue = new AuthenticationHeaderValue("Bearer",
                CurrentAccessToken);

            request.Headers.Add("User-Agent", "RedditLiveFeed");
            request.Headers.Authorization = authValue;

            return await _client.SendAsync(request);
        }
    }
}
