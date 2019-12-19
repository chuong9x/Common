using System;
using System.Collections.Generic;

namespace KeLi.Common.Tool.Cache
{
    /// <summary>
    /// Cache helper.
    /// </summary>
    public static class CacheHelper
    {
        private static readonly Dictionary<string, KeyValuePair<object, DateTime>> _data
            = new Dictionary<string, KeyValuePair<object, DateTime>>();

        /// <summary>
        /// Adds an item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static bool AddItem(string key, object data, int timeout = 30)
        {
            bool result;

            if (string.IsNullOrWhiteSpace(key))
                result = false;

            else if (string.IsNullOrWhiteSpace(data?.ToString()))
                result = false;

            else
            {
                _data[key] = new KeyValuePair<object, DateTime>(data, DateTime.Now);
                result = _data.ContainsKey(key);
            }

            return result;
        }

        /// <summary>
        /// Clears cache data.
        /// </summary>
        public static void Clear()
        {
            _data.Clear();
        }

        /// <summary>
        /// Removes the item by key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RemoveItem(string key)
        {
            return !string.IsNullOrWhiteSpace(key) && _data.Remove(key);
        }

        /// <summary>
        /// Gets the item by key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetItem<T>(string key)
        {
            T result;

            if (string.IsNullOrWhiteSpace(key))
                result = default;

            else if (!_data.ContainsKey(key))
                result = default;

            else if (_data[key].Value < DateTime.Now.AddSeconds(-30))
                result = default;

            else
                result = (T)_data[key].Key;

            return result;
        }

        /// <summary>
        /// Gets cache's item count.
        /// </summary>
        /// <returns></returns>
        public static int GetCount()
        {
            return _data.Count;
        }
    }
}
