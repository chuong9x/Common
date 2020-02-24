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
     |  |              Creation Time: 01/15/2020 03:48:41 PM |  |  |     |         |      |
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

using KeLi.Common.Revit.Geometry;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    ///     ModelLine builder.
    /// </summary>
    public static class ModelCurveBuilder
    {
        /// <summary>
        ///     Creates a new ModelCurve.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static ModelCurve CreateModelCurve(this Document doc, XYZ pt)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (pt is null)
                throw new ArgumentNullException(nameof(pt));

            var line = Line.CreateBound(XYZ.Zero, pt);

            return doc.CreateModelCurve(line, out _);
        }

        /// <summary>
        ///     Creates a new ModelCurve.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static ModelCurve CreateModelCurve(this Document doc, XYZ pt1, XYZ pt2)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (pt1 is null)
                throw new ArgumentNullException(nameof(pt1));

            if (pt2 is null)
                throw new ArgumentNullException(nameof(pt2));

            var line = Line.CreateBound(pt1, pt2);

            return doc.CreateModelCurve(line, out _);
        }

        /// <summary>
        ///     Creates a new ModelCurve.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="line"></param>
        /// <param name="sketchPlane"></param>
        /// <returns></returns>
        public static ModelCurve CreateModelCurve(this Document doc, Line line, out SketchPlane sketchPlane)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (line is null)
                throw new ArgumentNullException(nameof(line));

            var refAsix = XYZ.BasisZ;

            if (line.IsSameDirection(XYZ.BasisZ, -XYZ.BasisZ))
                refAsix = XYZ.BasisX;

            var normal = line.Direction.CrossProduct(refAsix).Normalize();

            var plane = Plane.CreateByNormalAndOrigin(normal, line.Origin);

            sketchPlane = SketchPlane.Create(doc, plane);

            if (doc.IsFamilyDocument)
                return doc.FamilyCreate.NewModelCurve(line, sketchPlane);

            return doc.Create.NewModelCurve(line, sketchPlane);
        }

        /// <summary>
        ///     Creates ModelCurve list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static List<ModelCurve> CreateModelCurveList(this Document doc, IEnumerable<XYZ> pts)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (pts is null)
                throw new ArgumentNullException(nameof(pts));

            return pts.Select(doc.CreateModelCurve).ToList();
        }

        /// <summary>
        ///     Creates ModelCurve list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static List<ModelCurve> CreateModelCurveList(this Document doc, params XYZ[] pts)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (pts is null)
                throw new ArgumentNullException(nameof(pts));

            return pts.Select(doc.CreateModelCurve).ToList();
        }

        /// <summary>
        ///     Creates ModelCurve list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<ModelCurve> CreateModelCurveList(this Document doc, IEnumerable<Line> lines)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (lines is null)
                throw new ArgumentNullException(nameof(lines));

            return lines.Select(s => doc.CreateModelCurve(s, out _)).ToList();
        }

        /// <summary>
        ///     Creates ModelCurve list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<ModelCurve> CreateModelCurveList(this Document doc, params Line[] lines)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (lines is null)
                throw new ArgumentNullException(nameof(lines));

            return lines.Select(s => doc.CreateModelCurve(s, out _)).ToList();
        }
    }
}