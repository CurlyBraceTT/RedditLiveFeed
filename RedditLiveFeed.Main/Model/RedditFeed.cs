using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditLiveFeed.Main.Model
{
    public class RedditFeed
    {
        class FeedComparer : IComparer<RedditEntry>
        {
            public int Compare(RedditEntry x, RedditEntry y)
            {
                return Comparer<long>.Default.Compare(y.CreatedUtc, x.CreatedUtc);
            }
        }

        public string Id { get; set; }
        public string LastEntryName { get; private set; }
        public int Size { get; private set; }

        private readonly HashSet<string> _hashset = new HashSet<string>();
        private readonly List<RedditEntry> _data = new List<RedditEntry>();

        public RedditFeed(int size = 10)
        {
            Size = size;
        }

        public void AddRange(IEnumerable<RedditEntry> entries)
        {
            foreach(var e in entries)
            {
                if(_hashset.Contains(e.Name))
                {
                    continue;
                }

                _hashset.Add(e.Name);
                _data.Add(e);
            }

            NormalizeData();
        }

        public void Add(RedditEntry entry)
        {
            if (_hashset.Contains(entry.Name))
            {
                return;
            }

            _hashset.Add(entry.Name);
            _data.Add(entry);
            NormalizeData();
        }

        public IEnumerable<RedditEntry> GetData(string lastEntryName = "")
        {
            if(string.IsNullOrEmpty(lastEntryName))
            {
                return _data;
            }

            return GetDataInner(lastEntryName) ?? new List<RedditEntry>();
        }

        private void NormalizeData()
        {
            _data.Sort(new FeedComparer());

            while(_data.Count > Size)
            {
                _hashset.Remove(_data[_data.Count - 1].Name);
                _data.RemoveAt(_data.Count - 1);
            }

            LastEntryName = _data.First().Name;
        }

        private IEnumerable<RedditEntry> GetDataInner(string lastEntryName = "")
        {
            foreach (var e in _data)
            {
                if (string.Equals(e.Name, lastEntryName, StringComparison.InvariantCultureIgnoreCase))
                {
                    yield break;
                }

                yield return e;
            }
        }
    }
}
