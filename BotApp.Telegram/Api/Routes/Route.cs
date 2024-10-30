using BotApp.Telegram.Api.Extensions;
using BotApp.Telegram.Api.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Diagnostics;

namespace BotApp.Telegram.Api.Routes
{
    internal class Route<TKey, TValue> 
        where TKey : notnull
        where TValue : notnull
    {
        private IDictionary<TKey, TValue> _routes = new Dictionary<TKey, TValue>(); 

        internal int Count => _routes.Count;

        internal TValue? this[TKey key] => Get(key);

        internal Route() { }

        internal bool Exist(TKey key) => _routes.ContainsKey(key.ThrowIfNull(nameof(key)));

        internal void Add(TKey key, TValue handler)
        {
#if DEBUG
            Debug.Print($"Add handler {Convert.ToString(handler)} for the key {Convert.ToString(key)}.");
#endif

            _routes.Add(key.ThrowIfNull(nameof(key)), handler.ThrowIfNull(nameof(handler)));
        }

        internal void Remove(TKey key)
        {
            if (_routes.ContainsKey(key.ThrowIfNull(nameof(key))))
                _routes.Remove(key);
        }

        internal TValue Get(TKey key)
        {
            if (_routes.ContainsKey(key.ThrowIfNull(nameof(key)))) 
                return _routes[key];

            throw new KeyNotFoundException();
        }
    }
}
