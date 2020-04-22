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
     |  |              Creation Time: 04/22/2020 08:05:20 PM |  |  |     |         |      |
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

using Autodesk.Revit.DB;

namespace KeLi.Common.Revit.Converters
{
    /// <summary>
    ///     About reference converter.
    /// </summary>
    public static class ReferenceConverter
    {
        /// <summary>
        ///     Converts the Reference set to the ReferenceArray.
        /// </summary>
        /// <param name="references"></param>
        /// <returns></returns>
        public static ReferenceArray ToReferArray(this IEnumerable<Reference> references)
        {
            if (references is null)
                throw new ArgumentNullException(nameof(references));

            var results = new ReferenceArray();

            foreach (var refer in references)
                results.Append(refer);

            return results;
        }

        /// <summary>
        ///     Converts the Reference set to the ReferenceArray.
        /// </summary>
        /// <param name="references"></param>
        /// <returns></returns>
        public static ReferenceArray ToReferArray(params Reference[] references)
        {
            if (references is null)
                throw new ArgumentNullException(nameof(references));

            var results = new ReferenceArray();

            foreach (var refer in references)
                results.Append(refer);

            return results;
        }

        /// <summary>
        ///     Converts the ReferenceArray to the Reference list.
        /// </summary>
        /// <param name="references"></param>
        /// <returns></returns>
        public static List<Reference> ToReferArray(this ReferenceArray references)
        {
            if (references is null)
                throw new ArgumentNullException(nameof(references));

            var results = new List<Reference>();

            foreach (Reference refer in references)
                results.Add(refer);

            return results;
        }
    }
}