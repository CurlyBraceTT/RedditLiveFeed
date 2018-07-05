using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RedditLiveFeed.Hubs;
using RedditLiveFeed.Model;

namespace RedditLiveFeed.Services
{
    public class NotifyService : INotifyService
    {
        private readonly IHubContext<RedditFeedHub> _hub;

        public NotifyService(IHubContext<RedditFeedHub> hub)
        {
            _hub = hub;
        }

        public async Task NotifyAsync(RedditFeed feed)
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
