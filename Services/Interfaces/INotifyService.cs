using System.Collections.Generic;
using System.Threading.Tasks;
using RedditLiveFeed.Model;

namespace RedditLiveFeed.Services.Interfaces
{
    public interface INotifyService
    {
        Task NotifyAsync(List<RedditEntry> feed);
    }
}
