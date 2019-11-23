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
     |  |              Creation Time: 10/30/2019 07:08:41 PM |  |  |     |         |      |
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
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Soap;

namespace KeLi.Common.Converter.Serialization
{
    /// <summary>
    /// A soap data serialization.
    /// </summary>
    public static class SoapUtil
    {
        /// <summary>
        ///  Serializes the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="filePath"></param>
        public static void Serialize<T>(FileInfo filePath, List<T> ts)
        {
            if (ts == null)
                throw new ArgumentNullException(nameof(ts));

            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            // Not support generic list, must convert to T type array.
            using (var fs = new FileStream(filePath.FullName, FileMode.Create))
                new SoapFormatter().Serialize(fs, ts.ToArray());
        }

        /// <summary>
        ///  Serializes the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="filePath"></param>
        public static void Serialize<T>(FileInfo filePath, T[] ts)
        {
            if (ts == null)
                throw new ArgumentNullException(nameof(ts));

            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            // Not support generic list, must convert to T type array.
            using (var fs = new FileStream(filePath.FullName, FileMode.Create))
                new SoapFormatter().Serialize(fs, ts.ToArray());
        }

        /// <summary>
        /// Deserializes the file text to the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<T> Deserialize<T>(FileInfo filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            using (var fs = new FileStream(filePath.FullName, FileMode.Open))
                return ((T[])new SoapFormatter().Deserialize(fs)).ToList();
        }
    }
}