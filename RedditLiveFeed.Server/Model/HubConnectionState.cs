using System.Collections.Generic;
using System.Threading.Channels;
using RedditLiveFeed.Main.Model;

namespace RedditLiveFeed.Server.Model
{
    public class HubConnectionState
    {
        public string LastEntryName { get; set; }
        public string ConnectionId { get; set; }
        public string FeedId { get; set; }
        public Channel<IEnumerable<RedditEntry>> StreamChannel { get; set; } 
    }
}
