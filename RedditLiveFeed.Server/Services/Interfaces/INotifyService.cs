using System.Collections.Generic;
using System.Threading.Tasks;
using RedditLiveFeed.Main.Model;

namespace RedditLiveFeed.Server.Services.Interfaces
{
    public interface INotifyService
    {
        Task NotifyAsync(List<RedditEntry> feed);
    }
}
