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
using System.Linq;

using Autodesk.Revit.DB;

namespace KeLi.Common.Revit.Converters
{
    /// <summary>
    ///     About curveArray converter.
    /// </summary>
    public static class CurveArrayConverter
    {
        /// <summary>
        ///     Converts the CurveArrArray to the CurveArray list.
        /// </summary>
        /// <param name="curveArrArray"></param>
        /// <returns></returns>
        public static List<CurveArray> ToCurveArrayList(this CurveArrArray curveArrArray)
        {
            if (curveArrArray is null)
                throw new ArgumentNullException(nameof(curveArrArray));

            var results = new List<CurveArray>();

            foreach (CurveArray curves in curveArrArray)
                results.Add(curves);

            return results;
        }

        /// <summary>
        ///     Converts the CurveLoop to the CurveArray.
        /// </summary>
        /// <param name="curveLoop"></param>
        /// <returns></returns>
        public static CurveArray ToCurveArray(this CurveLoop curveLoop)
        {
            if (curveLoop is null)
                throw new ArgumentNullException(nameof(curveLoop));

            var results = new CurveArray();

            foreach (var curve in curveLoop)
                results.Append(curve);

            return results;
        }

        /// <summary>
        ///     Converts the CurveLoop list to the CurveArray list.
        /// </summary>
        /// <param name="curveLoops"></param>
        /// <returns></returns>
        public static List<CurveArray> ToCurveArrayList(this IEnumerable<CurveLoop> curveLoops)
        {
            if (curveLoops is null)
                throw new ArgumentNullException(nameof(curveLoops));

            return curveLoops.Select(s => ToCurveArray(s)).ToList();
        }

        /// <summary>
        ///     Converts the CurveLoop list to the CurveArray list.
        /// </summary>
        /// <param name="curveLoops"></param>
        /// <returns></returns>
        public static List<CurveArray> ToCurveArrayList(this CurveLoop[] curveLoops)
        {
            if (curveLoops is null)
                throw new ArgumentNullException(nameof(curveLoops));

            return curveLoops.Select(s => s.ToCurveArray()).ToList();
        }

        /// <summary>
        ///     Converts the Curve list to the CurveArray.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveArray ToCurveArray<T>(this IEnumerable<T> curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            var results = new CurveArray();

            foreach (var curve in curves)
                results.Append(curve);

            return results;
        }

        /// <summary>
        ///     Converts the Curve list to the CurveArray.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveArray ToCurveArray<T>(params T[] curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            var results = new CurveArray();

            foreach (var curve in curves)
                results.Append(curve);

            return results;
        }

        /// <summary>
        ///     Converts the Curve list to the CurveArray list.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static List<CurveArray> ToCurveArrayList<T>(this IEnumerable<T> curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            var ary = new CurveArray();

            foreach (var curve in curves)
                ary.Append(curve);

            return new List<CurveArray>
            {
                ary
            };
        }

        /// <summary>
        ///     Converts the Curve list to the CurveArray list.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static List<CurveArray> ToCurveArrayList<T>(params T[] curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            var ary = new CurveArray();

            foreach (var curve in curves)
                ary.Append(curve);

            return new List<CurveArray>
            {
                ary
            };
        }
    }
}