using System.Threading.Tasks;
using RedditLiveFeed.Model;

namespace RedditLiveFeed.Services
{
    public interface INotifyService
    {
        Task NotifyAsync(RedditFeed feed);
    }
}
