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

namespace KeLi.Common.Tool.Cache
{
    /// <summary>
    /// Easy cache helper.
    /// </summary>
    public static class EasyCacheHelper
    {
        /// <summary>
        /// Restores timeout.
        /// </summary>
        private static int _timeout = 30;

        /// <summary>
        /// Restores cache data.
        /// </summary>
        private static readonly Dictionary<string, KeyValuePair<object, DateTime>> _data;
           
        /// <summary>
        /// Inits cache.
        /// </summary>
        static EasyCacheHelper()
        {
            _data  = new Dictionary<string, KeyValuePair<object, DateTime>>();
        }

        /// <summary>
        /// Adds an item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool AddItem(string key, object data)
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
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetItem(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return default;

            if (!_data.ContainsKey(key))
                return  default;

            if (_data[key].Value < DateTime.Now.AddSeconds(-_timeout))
                return default;

            return _data[key].Key;
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

            else if (_data[key].Value < DateTime.Now.AddSeconds(-_timeout))
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

        /// <summary>
        /// Sets cache item's timeout.
        /// </summary>
        /// <param name="timeout"></param>
        public static void SetTimeout(int timeout = 30)
        {
            _timeout = timeout;
        }
    }
}
