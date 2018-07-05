using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedditLiveFeed.Utils
{
    public class RedditAuthConfiguration
    {
        public string ClientId { get; set; }
        public string Secret { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
