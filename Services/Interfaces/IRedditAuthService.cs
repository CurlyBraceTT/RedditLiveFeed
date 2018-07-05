using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedditLiveFeed.Services.Interfaces
{
    public interface IRedditAuthService
    {
        Task Authenticate();
        Task RefreshAccess();
        bool IsAuthenticated();
        string CurrentAccessToken { get; }
    }
}
