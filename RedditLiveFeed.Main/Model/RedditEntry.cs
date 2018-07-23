using System;
using Newtonsoft.Json;

namespace RedditLiveFeed.Main.Model
{
    [JsonConverter(typeof(RedditEntryJsonConverter))]
    public class RedditEntry
    {
        public string Name { get; set; }
        public string Title { get; set; }
        [JsonProperty("created_utc")]
        public long CreatedUtc { get; set; }
        public string Thumbnail { get; set; }
        public string Url { get; set; }
        public string Permalink { get; set; }
        public string Subreddit { get; set; }

        public DateTime CreatedParsed => DateTimeOffset.FromUnixTimeSeconds(CreatedUtc).UtcDateTime;
        public string SubredditUrl => "https://reddit.com/r/" + Subreddit;
        public string EntryRedditUrl => "https://reddit.com" + Permalink;
        public string RedditUrl => "https://reddit.com" + Permalink;
    }
}
