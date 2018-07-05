using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RedditLiveFeed.Model;
using RedditLiveFeed.Services;

namespace RedditLiveFeed.Hubs
{
    public class RedditFeedHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;

            //var test = Channel.CreateUnbounded<int>();

            var id = Context.ConnectionId;

            // use this method to pass connection Id to the injected service bus

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
