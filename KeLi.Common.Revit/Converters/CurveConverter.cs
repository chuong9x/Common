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
    ///     About curve converter.
    /// </summary>
    public static class CurveConverter
    {
        /// <summary>
        ///     Converts the CurveLoop list to the Curve list.
        /// </summary>
        /// <param name="curveLoops"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(this IEnumerable<CurveLoop> curveLoops)
        {
            if (curveLoops is null)
                throw new ArgumentNullException(nameof(curveLoops));

            return curveLoops.SelectMany(s => s).ToList();
        }

        /// <summary>
        ///     Converts the CurveLoop list to the Curve list.
        /// </summary>
        /// <param name="curveLoops"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(params CurveLoop[] curveLoops)
        {
            if (curveLoops is null)
                throw new ArgumentNullException(nameof(curveLoops));

            return curveLoops.SelectMany(s => s).ToList();
        }

        /// <summary>
        ///     Converts the CurveArrArray to the Curve list.
        /// </summary>
        /// <param name="curveArrArray"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(this CurveArrArray curveArrArray)
        {
            if (curveArrArray is null)
                throw new ArgumentNullException(nameof(curveArrArray));

            var results = new List<Curve>();

            foreach (CurveArray curves in curveArrArray)
                results.AddRange(curves.ToCurveList());

            return results;
        }

        /// <summary>
        ///     Converts the CurveArray list to the Curve list.
        /// </summary>
        /// <param name="curveArrays"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(this IEnumerable<CurveArray> curveArrays)
        {
            if (curveArrays is null)
                throw new ArgumentNullException(nameof(curveArrays));

            return curveArrays.SelectMany(s => s.ToCurveList()).ToList();
        }

        /// <summary>
        ///     Converts the CurveArray list to the Curve list.
        /// </summary>
        /// <param name="curveArrays"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(params CurveArray[] curveArrays)
        {
            if (curveArrays is null)
                throw new ArgumentNullException(nameof(curveArrays));

            return curveArrays.SelectMany(s => s.ToCurveList()).ToList();
        }

        /// <summary>
        ///     Converts the CurveLoop to the Curve list.
        /// </summary>
        /// <param name="curveLoop"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(this CurveLoop curveLoop)
        {
            if (curveLoop is null)
                throw new ArgumentNullException(nameof(curveLoop));

            var results = new List<Curve>();

            foreach (var curve in curveLoop)
                results.Add(curve);

            return results;
        }

        /// <summary>
        ///     Converts the CurveArray to the Curve list.
        /// </summary>
        /// <param name="curveArray"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(this CurveArray curveArray)
        {
            if (curveArray is null)
                throw new ArgumentNullException(nameof(curveArray));

            var results = new List<Curve>();

            foreach (Curve curve in curveArray)
                results.Add(curve);

            return results;
        }
    }
}