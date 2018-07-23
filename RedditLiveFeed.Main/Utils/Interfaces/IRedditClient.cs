using System.Net.Http;
using System.Threading.Tasks;

namespace RedditLiveFeed.Main.Utils.Interfaces
{
    /// <summary>
    /// Produced by IRedditClient Factory
    /// Authorized reddit client, ready to work,
    /// Will Replace RedditAuthService
    /// </summary>
    public interface IRedditClient
    {
        Task Authenticate();
        Task RefreshAccess();
        bool Authenticated { get; }
        bool Expired { get; }
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}
