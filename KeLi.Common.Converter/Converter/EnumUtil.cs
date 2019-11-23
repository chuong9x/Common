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
     |  |              Creation Time: 11/15/2019 02:43:59 PM |  |  |     |         |      |
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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KeLi.Common.Converter.Converter
{
    /// <summary>
    /// Enum Utility.
    /// </summary>
    public static class EnumUtil
    {
        /// <summary>
        /// Trys parse the value to enum value by display attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParseByDisplayAttr<T>(string value, out T result) where T : Enum
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var descs = GetDisplayEnumDict<T>();

            if (!descs.TryGetValue(value, out result))
                throw new InvalidEnumArgumentException();

            return true;
        }

        /// <summary>
        /// Trys parse the value to enum value by description attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParseByDescAttr<T>(string value, out T result) where T : Enum
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var descs = GetDescriptionEnumDict<T>();

            if (!descs.TryGetValue(value, out result))
                throw new InvalidEnumArgumentException();

            return true;
        }

        /// <summary>
        /// Gets a dictionary composed of display name and enum item value from enum type.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, T> GetDisplayEnumDict<T>() where T : Enum
        {
            var results = new Dictionary<string, T>();
            var values = Enum.GetValues(typeof(T));

            foreach (T value in values)
            {
                var member = typeof(T).GetMember(value.ToString()).FirstOrDefault();
                var atts = member.GetCustomAttributes(typeof(DisplayAttribute), false);
                var name = (atts.FirstOrDefault() as DisplayAttribute)?.Name;

                if (name != null)
                    results.Add(name, value);
            }

            return results;
        }

        /// <summary>
        /// Gets a dictionary composed of description and enum item value from enum type.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, T> GetDescriptionEnumDict<T>() where T : Enum
        {
            var results = new Dictionary<string, T>();
            var values = Enum.GetValues(typeof(T));

            foreach (T value in values)
            {
                var member = typeof(T).GetMember(value.ToString()).FirstOrDefault();
                var atts = member.GetCustomAttributes(typeof(DescriptionAttribute), false);
                var name = (atts.FirstOrDefault() as DescriptionAttribute)?.Description;

                if (name != null)
                    results.Add(name, value);
            }

            return results;
        }
    }
}