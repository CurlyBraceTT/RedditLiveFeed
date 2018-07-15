using System;
using System.Collections.Generic;

namespace RedditLiveFeed.Model
{
    public class RedditListing
    {
        public string After { get; set; }
        public string Before { get; set; }
        public List<RedditEntry> Children { get; set; }
    }
}
