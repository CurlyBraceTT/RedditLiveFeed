using System.Threading.Tasks;
using RedditLiveFeed.Main.Model;

namespace RedditLiveFeed.Main.Services.Interfaces
{
    public interface IRedditApiService
    {
        Task<RedditListing> GetNew(string before = "", string after = "", int limit = 0);
    }
}
