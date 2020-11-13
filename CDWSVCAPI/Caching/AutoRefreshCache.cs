using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDWSVCAPI.Caching
{
    public abstract class AutoRefreshCache<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, TValue> _entries = new ConcurrentDictionary<TKey, TValue>();

        protected TimeSpan _interval { get; set; }

        protected ILogger _logger { get; set; }

        protected AutoRefreshCache(TimeSpan interval, ILogger logger)
        {
            _interval = interval;
            _logger = logger;
            var timer = new System.Timers.Timer();
            timer.Interval = _interval.TotalMilliseconds;
            timer.AutoReset = true;
            timer.Elapsed += (o, e) =>
            {
                ((System.Timers.Timer)o).Stop();
                RefreshAll();
                ((System.Timers.Timer)o).Start();
            };
            timer.Start();
        }

        public TValue Get(TKey key)
        {
            return _entries.GetOrAdd(key, k => Load(k));
        }

        public TValue GetIfExists(TKey key)
        {
            return _entries.ContainsKey(key) ? _entries[key] : default;
        }

        public TValue Remove(TKey key)
        {
            if (_entries.TryRemove(key, out TValue removed))
            {
                return removed;
            }
            return default;
        }

        public void RefreshAll()
        {
            var keys = _entries.Keys;
            foreach (var key in keys)
            {
                _entries.AddOrUpdate(key, k => Load(key), (k, v) => Load(key));
            }
        }

        protected abstract TValue Load(TKey key);
    }
}