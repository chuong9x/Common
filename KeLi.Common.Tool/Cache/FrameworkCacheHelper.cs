/*
 * MIT License
 *
 * Copyright(c) 2019 KeLi
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

/*
             ,---------------------------------------------------,              ,---------,
        ,----------------------------------------------------------,          ,"        ,"|
      ,"                                                         ,"|        ,"        ,"  |
     +----------------------------------------------------------+  |      ,"        ,"    |
     |  .----------------------------------------------------.  |  |     +---------+      |
     |  | C:\>FILE -INFO                                     |  |  |     | -==----'|      |
     |  |                                                    |  |  |     |         |      |
     |  |                                                    |  |  |/----|`---=    |      |
     |  |              Author: KeLi                          |  |  |     |         |      |
     |  |              Email: kelistudy@163.com              |  |  |     |         |      |
     |  |              Creation Time: 12/20/2019 12:05:41 PM |  |  |     |         |      |
     |  | C:\>_                                              |  |  |     | -==----'|      |
     |  |                                                    |  |  |   ,/|==== ooo |      ;
     |  |                                                    |  |  |  // |(((( [66]|    ,"
     |  `----------------------------------------------------'  |," .;'| |((((     |  ,"
     +----------------------------------------------------------+  ;;  | |         |,"
        /_)_________________________________________________(_/  //'   | +---------+
           ___________________________/___  `,
          /  oooooooooooooooo  .o.  oooo /,   \,"-----------
         / ==ooooooooooooooo==.o.  ooo= //   ,`\--{)B     ,"
        /_==__==========__==_ooo__ooo=_/'   /___________,"
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace KeLi.Common.Tool.Cache
{
    /// <summary>
    ///     .Net framework cache helper.
    /// </summary>
    public sealed class FrameworkCacheHelper
    {
        /// <summary>
        ///     Restores cache data.
        /// </summary>
        private readonly ObjectCache _data = MemoryCache.Default;

        /// <summary>
        ///     Gets cache's item by index.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                if (key is null)
                    throw new ArgumentNullException(nameof(key));

                return _data.Get(key);
            }

            set
            {
                if (key is null)
                    throw new ArgumentNullException(nameof(key));

                AddItem(key, value);
            }
        }

        /// <summary>
        ///     Adds an item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool AddItem(string key, object data, int timeout = 30)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            if (data is null)
                throw new ArgumentNullException(nameof(data));

            bool result;

            if (string.IsNullOrWhiteSpace(key))
                result = false;

            else if (string.IsNullOrWhiteSpace(data.ToString()))
                result = false;

            else
            {
                var cip = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now.AddMinutes(timeout) };

                result = _data.Add(new CacheItem(key, data), cip);
            }

            return result;
        }

        /// <summary>
        ///     Clears cache data.
        /// </summary>
        public void Clear()
        {
            foreach (var item in _data)
                RemoveItem(item.Key);
        }

        /// <summary>
        ///     Removes the item by key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object RemoveItem(string key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            object result;

            if (string.IsNullOrWhiteSpace(key))
                result = null;

            else if (!_data.Contains(key))
                result = null;

            else
                result = _data.Remove(key);

            return result;
        }

        /// <summary>
        ///     Gets the item by key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetItem(string key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            if (string.IsNullOrWhiteSpace(key))
                return default;

            if (!_data.Contains(key))
                return default;

            return _data[key];
        }

        /// <summary>
        ///     Gets the item by key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetItem<T>(string key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            T result;

            if (string.IsNullOrWhiteSpace(key))
                result = default;

            else if (!_data.Contains(key))
                result = default;

            else
                result = (T)_data[key];

            return result;
        }

        /// <summary>
        ///     Gets the item by pattern.
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public List<object> GetItemList(string pattern)
        {
            if (pattern is null)
                throw new ArgumentNullException(nameof(pattern));

            List<object> results;

            var regex = new Regex(pattern);

            var keys = _data.Where(w => regex.IsMatch(w.Key)).Select(s => s.Key).ToList();

            if (keys.Count == 0)
                results = null;

            else
            {
                results = new List<object>();

                results.AddRange(keys.Select(RemoveItem));
            }

            return results;
        }

        /// <summary>
        ///     Gets cache's item count.
        /// </summary>
        /// <returns></returns>
        public long GetCount()
        {
            return _data.GetCount();
        }
    }
}