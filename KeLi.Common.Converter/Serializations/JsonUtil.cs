﻿/*
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
using System.IO;
using System.Web.Script.Serialization;

namespace KeLi.Common.Converter.Serializations
{
    /// <summary>
    ///     A json data serialization.
    /// </summary>
    public class JsonUtil
    {
        /// <summary>
        ///     Serializes the object that may be a entity or a collection.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="t"></param>
        public static void Serialize<T>(string filePath, T t) where T : class
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            if (t is null)
                throw new ArgumentNullException(nameof(t));

            using (var sw = new StreamWriter(filePath))
                sw.Write(new JavaScriptSerializer().Serialize(t));
        }

        /// <summary>
        ///     Serializes the object that may be a entity or a collection.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="t"></param>
        public static void Serialize<T>(FileInfo filePath, T t) where T : class
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            if (t is null)
                throw new ArgumentNullException(nameof(t));

            Serialize(filePath.FullName, t);
        }

        /// <summary>
        ///     Deserializes the file text to T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string filePath) where T : class
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            using (var sr = new StreamReader(filePath))
                return new JavaScriptSerializer().Deserialize<T>(sr.ReadToEnd());
        }

        /// <summary>
        ///     Deserializes the file text to T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T Deserialize<T>(FileInfo filePath) where T : class
        {
            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            return Deserialize<T>(filePath.FullName);
        }
    }
}