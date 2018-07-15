using System.Threading.Tasks;
using RedditLiveFeed.Model;

namespace RedditLiveFeed.Services.Interfaces
{
    public interface IRedditApiService
    {
        Task<RedditListing> GetNew(string before = "", string after = "", int limit = 0);
    }
}
