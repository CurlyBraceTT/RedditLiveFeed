using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RedditLiveFeed.Model;
using RedditLiveFeed.Services.Interfaces;

namespace RedditLiveFeed.Hubs
{
    public class RedditFeedHub : Hub
    {
        private readonly IConnectionStateService _stateService;
        private readonly IRedditFeedService _feedService;

        public RedditFeedHub(IConnectionStateService stateService,
            IRedditFeedService feedService)
        {
            _stateService = stateService;
            _feedService = feedService;
        }

        public ChannelReader<IEnumerable<RedditEntry>> Feed(string feedId)
        {
            var channel = Channel.CreateUnbounded<IEnumerable<RedditEntry>>();

            var connection = new HubConnectionState
            {
                ConnectionId = Context.ConnectionId,
                FeedId = feedId,
                StreamChannel = channel,
            };

            if (_feedService.TryGetFeed(feedId, out var feed))
            {
                var data = feed.GetData();
                if (data.Count() > 0)
                {
                    _ = connection.StreamChannel.Writer.WriteAsync(data);
                    connection.LastEntryName = feed.LastEntryName;
                }
            }
            else
            {
                _feedService.AddFeed(feedId, new RedditFeed
                {
                    Id = feedId
                });
            }

            _stateService.AddConnection(connection);

            return channel.Reader;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _stateService.RemoveConnection(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
