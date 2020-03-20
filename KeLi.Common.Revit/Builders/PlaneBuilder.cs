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

using Autodesk.Revit.DB;

using KeLi.Common.Revit.Geometry;

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
        /// <param name="pt"></param>
        /// <returns></returns>
        public static Plane CreatePlane(this XYZ pt)
        {
            if (pt is null)
                throw new ArgumentNullException(nameof(pt));

            if (pt == XYZ.Zero)
                throw new ArgumentException("The point cannot be zero point!");

            return Line.CreateBound(XYZ.Zero, pt).CreatePlane();
        }

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

            if (line.IsSameDirection(XYZ.BasisZ, -XYZ.BasisZ))
                refAsix = XYZ.BasisX;

            var normal = line.Direction.CrossProduct(refAsix).Normalize();

            return normal.CreatePlane(refAsix);
        }

        /// <summary>
        ///     Creates a new plane.
        /// </summary>
        /// <param name="normal"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static Plane CreatePlane(this XYZ normal, XYZ origin)
        {
            #if R2016
            return new Plane(normal, line.Origin);
            #endif
            #if !R2016

            return Plane.CreateByNormalAndOrigin(normal, origin);
            #endif
        }

        /// <summary>
        ///     Creates a new sketch plane.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static SketchPlane CreateSketchPlane(this Document doc, XYZ pt)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (pt is null)
                throw new ArgumentNullException(nameof(pt));

            return SketchPlane.Create(doc, pt.CreatePlane());
        }

        /// <summary>
        ///     Creates a new sketch plane.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static SketchPlane CreateSketchPlane(this Document doc, Line line)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (line is null)
                throw new ArgumentNullException(nameof(line));

            return SketchPlane.Create(doc, line.CreatePlane());
        }

        /// <summary>
        ///     Creates a new sketch plane.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static SketchPlane CreateSketchPlane(this Document doc, Plane plane)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (plane is null)
                throw new ArgumentNullException(nameof(plane));

            return SketchPlane.Create(doc, plane);
        }
    }
}