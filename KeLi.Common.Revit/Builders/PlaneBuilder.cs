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

using Autodesk.Revit.DB;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    ///     Plane builder.
    /// </summary>
    public static class PlaneBuilder
    {
        /// <summary>
        ///     Creates a new plane.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Plane CreatePlane(this Line line)
        {
            if (line is null)
                throw new ArgumentNullException(nameof(line));

            var refAsix = XYZ.BasisZ;

            if (Math.Abs(line.Direction.AngleTo(XYZ.BasisZ)) < 1e-6)
                refAsix = XYZ.BasisX;

            else if (Math.Abs(line.Direction.AngleTo(-XYZ.BasisZ)) < 1e-6)
                refAsix = XYZ.BasisX;

            var normal = line.Direction.CrossProduct(refAsix).Normalize();

            return normal.CreatePlane(refAsix);
        }

        /// <summary>
        ///     Creates a new plane.
        /// </summary>
        /// <param name="normal"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Plane CreatePlane(this XYZ normal, XYZ point)
        {
            #if R2016
            return new Plane(normal, point);

            #endif

            #if !R2016

            return Plane.CreateByNormalAndOrigin(normal, point);

            #endif
        }

        /// <summary>
        ///     Creates a new sketch plane.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="loftingPath"></param>
        /// <returns></returns>
        public static SketchPlane CreateSketchPlane(this Document doc, CurveArray loftingPath)
        {
            var curve1 = loftingPath.get_Item(0);

            var curve2 = loftingPath.get_Item(1);

            var p0 = curve1.GetEndPoint(0);

            var p1 = curve1.GetEndPoint(1);

            var p2 = curve2.GetEndPoint(1);

            var normal = p0.CrossProduct(p1);

            var plane = normal.CreatePlane(p2);

            return SketchPlane.Create(doc, plane);
        }
    }
}