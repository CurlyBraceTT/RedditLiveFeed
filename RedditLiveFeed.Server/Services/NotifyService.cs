using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RedditLiveFeed.Server.Hubs;
using RedditLiveFeed.Main.Model;
using RedditLiveFeed.Server.Services.Interfaces;

namespace RedditLiveFeed.Server.Services
{
    public class NotifyService : INotifyService
    {
        private readonly IHubContext<RedditFeedHub> _hub;

        public NotifyService(IHubContext<RedditFeedHub> hub)
        {
            _hub = hub;
        }

        public async Task NotifyAsync(List<RedditEntry> feed)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var serialized = JsonConvert.SerializeObject(feed, settings);

            await _hub.Clients.All.SendAsync("RefreshFeed", serialized);
        }
    }
}
