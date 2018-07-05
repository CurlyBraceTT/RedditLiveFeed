using System.Threading.Tasks;
using RedditLiveFeed.Model;

namespace RedditLiveFeed.Services.Interfaces
{
    public interface IRedditFeedService
    {
        Task<RedditFeed> GetNew(string after = "");
    }
}
