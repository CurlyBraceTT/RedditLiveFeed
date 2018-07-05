using System.Xml.Serialization;

namespace RedditLiveFeed.Model
{
    public class RedditEntry
    {
        [XmlElement("title")]
        public string Title { get; set; }
        [XmlElement("content")]
        public string Content { get; set; }
    }
}
