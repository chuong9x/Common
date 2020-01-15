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
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace KeLi.Common.Revit.Converters
{
    /// <summary>
    /// Type converter.
    /// </summary>
    public static class TypeConverter
    {
        /// <summary>
        /// Convers the space curve to the plane line.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Line ToPlaneLine(this Curve line)
        {
            if (line == null)
                throw new ArgumentNullException(nameof(line));

            var p1 = line.GetEndPoint(0).ToPlanePoint();
            var p2 = line.GetEndPoint(1).ToPlanePoint();

            return Line.CreateBound(p1, p2);
        }

        /// <summary>
        /// Convers the space point to the plane point.
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static XYZ ToPlanePoint(this XYZ pt)
        {
            if (pt == null)
                throw new ArgumentNullException(nameof(pt));

            return new XYZ(pt.X, pt.Y, 0);
        }

        /// <summary>
        /// Converts the reference set to the reference array.
        /// </summary>
        /// <param name="refs"></param>
        /// <returns></returns>
        public static ReferenceArray ToReferArray(this List<Reference> refs)
        {
            if (refs == null)
                throw new ArgumentNullException(nameof(refs));

            var results = new ReferenceArray();

            foreach (var refer in refs)
                results.Append(refer);

            return results;
        }

        /// <summary>
        /// Converts the curve array set to the curve arr array.
        /// </summary>
        /// <param name="curvess"></param>
        /// <returns></returns>
        public static CurveArrArray ToCurveArrArray(this List<CurveArray> curvess)
        {
            if (curvess == null)
                throw new ArgumentNullException(nameof(curvess));

            var results = new CurveArrArray();

            foreach (var curves in curvess)
                results.Append(curves);

            return results;
        }

        /// <summary>
        /// Converts the curve set to the curve array.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveArray ToCurveArray(this List<Curve> curves)
        {
            if (curves == null)
                throw new ArgumentNullException(nameof(curves));

            var results = new CurveArray();

            foreach (var curve in curves)
                results.Append(curve);

            return results;
        }

        /// <summary>
        /// Gets the round point with custom precision.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static XYZ GetRoundPoint(this XYZ point, int precision = 4)
        {
            if (point == null)
                throw new ArgumentNullException(nameof(point));

            return new XYZ(Math.Round(point.X, precision), Math.Round(point.Y, precision), Math.Round(point.Z, precision));
        }
    }
}