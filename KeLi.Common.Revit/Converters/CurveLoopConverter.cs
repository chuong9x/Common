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
    ///     About curveLoop converter.
    /// </summary>
    public static class CurveLoopConverter
    {
        /// <summary>
        ///     Converts the CurveArray to the CurveLoop.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveLoop ToCurveLoop<T>(this IEnumerable<T> curves) where T : Curve
        {
            var curveList = curves.ToList();

            if (curves == null || curveList.Count == 0)
                throw new NullReferenceException(nameof(curveList));

            var curveLoop = new CurveLoop();

            var endPt = curveList[0]?.GetEndPoint(1);

            curveLoop.Append(curveList[0]);

            curveList[0] = null;

            // If computing count equals curveLoop count, it should break.
            // Because, the curveLoop cannot find valid curve to append.
            var count = 0;

            while (count < curveList.Count && curveLoop.Count() < curveList.Count)
            {
                for (var i = 0; i < curveList.Count; i++)
                {
                    if (curveList[i] == null)
                        continue;

                    var p0 = curveList[i].GetEndPoint(0);

                    var p1 = curveList[i].GetEndPoint(1);

                    if (p0.IsAlmostEqualTo(endPt))
                    {
                        endPt = p1;

                        curveLoop.Append(curveList[i]);

                        curveList[i] = null;
                    }

                    // The curve should be reversed.
                    else if (p1.IsAlmostEqualTo(endPt))
                    {
                        endPt = p0;

                        curveLoop.Append(curveList[i].CreateReversed());

                        curveList[i] = null;
                    }
                }

                count++;
            }

            return curveLoop;
        }

        /// <summary>
        ///     Converts the Curve list to the CurveLoop.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveLoop ToCurveLoop<T>(params T[] curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            return curves.ToCurveLoop();
        }

        /// <summary>
        ///     Converts the Curve list to the CurveLoop list.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static List<CurveLoop> ToCurveLoopList<T>(this IEnumerable<T> curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            var loop = curves.ToCurveLoop();

            return new List<CurveLoop>
            {
                loop
            };
        }

        /// <summary>
        ///     Converts the Curve list to the CurveLoop list.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static List<CurveLoop> ToCurveLoopList<T>(params T[] curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            var loop = curves.ToCurveLoop();

            return new List<CurveLoop>
            {
                loop
            };
        }

        /// <summary>
        ///     Converts the CurveArray to the CurveLoop.
        /// </summary>
        /// <param name="curveArray"></param>
        /// <returns></returns>
        public static CurveLoop ToCurveLoop(this CurveArray curveArray)
        {
            if (curveArray is null)
                throw new ArgumentNullException(nameof(curveArray));

            return curveArray.ToCurveList().ToCurveLoop();
        }

        /// <summary>
        ///     Converts the CurveArray list to the CurveLoop list.
        /// </summary>
        /// <param name="curveArrays"></param>
        /// <returns></returns>
        public static List<CurveLoop> ToCurveLoopList(this IEnumerable<CurveArray> curveArrays)
        {
            if (curveArrays is null)
                throw new ArgumentNullException(nameof(curveArrays));

            var results = new CurveArrArray();

            foreach (var curves in curveArrays)
                results.Append(curves);

            return results.ToCurveLoopList();
        }

        /// <summary>
        ///     Converts the CurveArray list to the CurveLoop list.
        /// </summary>
        /// <param name="curveArrays"></param>
        /// <returns></returns>
        public static List<CurveLoop> ToCurveLoopList(params CurveArray[] curveArrays)
        {
            if (curveArrays is null)
                throw new ArgumentNullException(nameof(curveArrays));

            var results = new CurveArrArray();

            foreach (var curves in curveArrays)
                results.Append(curves);

            return results.ToCurveLoopList();
        }

        /// <summary>
        ///     Converts the CurveArrArray to the CurveLoop list.
        /// </summary>
        /// <param name="curveArrArray"></param>
        /// <returns></returns>
        public static List<CurveLoop> ToCurveLoopList(this CurveArrArray curveArrArray)
        {
            if (curveArrArray is null)
                throw new ArgumentNullException(nameof(curveArrArray));

            var results = new List<CurveLoop>();

            foreach (CurveArray curves in curveArrArray)
                results.Add(curves.ToCurveLoop());

            return results;
        }
    }
}