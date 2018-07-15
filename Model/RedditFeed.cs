using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RedditLiveFeed.Model
{
    public class RedditFeed
    {
        class DescComparer<T> : IComparer<T>
        {
            public int Compare(T x, T y)
            {
                return Comparer<T>.Default.Compare(y, x);
            }
        }

        public string Id { get; set; }
        public string LastEntryName
        {
            get
            {
                if(_data.Count == 0)
                {
                    return string.Empty;
                }

                return _data.First().Value.Name;
            }
        }

        public int Size { get; private set; }
        private readonly SortedList<long, RedditEntry> _data;

        public RedditFeed(int size = 10)
        {
            Size = size;
            _data = new SortedList<long, RedditEntry>(size, new DescComparer<long>());
        }

        public void AddRange(IEnumerable<RedditEntry> entries)
        {
            foreach(var e in entries)
            {
                _data.Add(e.CreatedUtc, e);
            }
        }

        public void Add(RedditEntry entry)
        {
            _data.Add(entry.CreatedUtc, entry);

            //TODO Remove Items from list
        }

        public IEnumerable<RedditEntry> GetData(string lastEntryName = "")
        {
            if(string.IsNullOrEmpty(lastEntryName))
            {
                return _data.Values;
            }

            return GetDataInner(lastEntryName) ?? new List<RedditEntry>();
        }

        private IEnumerable<RedditEntry> GetDataInner(string lastEntryName = "")
        {
            foreach (var e in _data)
            {
                if (string.Equals(e.Value.Name, lastEntryName, StringComparison.InvariantCultureIgnoreCase))
                {
                    yield break;
                }

                yield return e.Value;
            }
        }
    }
}
