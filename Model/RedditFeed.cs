using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RedditLiveFeed.Model
{
    [XmlRoot("feed", Namespace = "http://www.w3.org/2005/Atom")]
    public class RedditFeed
    {
        [XmlElement("id")]
        public string Id { get; set; }
        [XmlElement("title")]
        public string Title { get; set; }
        [XmlElement("updated")]
        public DateTime Updated { get; set; }

        [XmlElement("entry")]
        public List<RedditEntry> Entries { get; set; }
    }
}
