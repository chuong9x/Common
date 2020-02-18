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
using System.ComponentModel;
using System.Linq;
using Autodesk.Revit.DB;
using KeLi.Common.Revit.Converters;
using KeLi.Common.Revit.Geometry;

namespace KeLi.Common.Revit.Relations
{
    /// <summary>
    ///     About a solid and a solid relationship.
    /// </summary>
    public static class SolidRelaction
    {
        /// <summary>
        ///     Gets the result of fast plane rejection of the box.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="elm1"></param>
        /// <param name="elm2"></param>
        /// <returns></returns>
        public static bool RejectPlaneBox(this Document doc, Element elm1, Element elm2)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (elm1 is null)
                throw new ArgumentNullException(nameof(elm1));

            if (elm2 is null)
                throw new ArgumentNullException(nameof(elm2));

            var box1 = elm1.GetRoundBox(doc);
            var box2 = elm2.GetRoundBox(doc);
            var v1 = box1.Min.X <= box2.Max.X;
            var v2 = box1.Max.X >= box2.Min.X;
            var v3 = box1.Min.Y <= box2.Max.Y;
            var v4 = box1.Max.Y >= box2.Min.Y;

            return v1 && v2 && v3 && v4;
        }

        /// <summary>
        ///     Gets the result of fast plane rejection of the box.
        /// </summary>
        /// <param name="box1"></param>
        /// <param name="box2"></param>
        /// <returns></returns>
        public static bool RejectPlaneBox(this BoundingBoxXYZ box1, BoundingBoxXYZ box2)
        {
            if (box1 is null)
                throw new ArgumentNullException(nameof(box1));

            if (box2 is null)
                throw new ArgumentNullException(nameof(box2));

            box1 = box1.GetRoundBox();
            box2 = box2.GetRoundBox();

            var v1 = box1.Min.X <= box2.Max.X;
            var v2 = box1.Max.X >= box2.Min.X;
            var v3 = box1.Min.Y <= box2.Max.Y;
            var v4 = box1.Max.Y >= box2.Min.Y;

            return v1 && v2 && v3 && v4;
        }

        /// <summary>
        ///     Gets the result of fast space rejection of the box.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="elm1"></param>
        /// <param name="elm2"></param>
        /// <returns></returns>
        public static bool RejectSpaceBox(this Document doc, Element elm1, Element elm2)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (elm1 is null)
                throw new ArgumentNullException(nameof(elm1));

            if (elm2 is null)
                throw new ArgumentNullException(nameof(elm2));

            var box1 = elm1.GetRoundBox(doc);
            var box2 = elm2.GetRoundBox(doc);
            var v1 = box1.Min.X <= box2.Max.X;
            var v2 = box1.Max.X >= box2.Min.X;
            var v3 = box1.Min.Y <= box2.Max.Y;
            var v4 = box1.Max.Y >= box2.Min.Y;
            var v5 = box1.Min.Z <= box2.Max.Z;
            var v6 = box1.Max.Z >= box2.Min.Z;

            return v1 && v2 && v3 && v4 && v5 && v6;
        }

        /// <summary>
        ///     Gets the result of fast space rejection of the box.
        /// </summary>
        /// <param name="box1"></param>
        /// <param name="box2"></param>
        /// <returns></returns>
        public static bool RejectSpaceBox(this BoundingBoxXYZ box1, BoundingBoxXYZ box2)
        {
            if (box1 is null)
                throw new ArgumentNullException(nameof(box1));

            if (box2 is null)
                throw new ArgumentNullException(nameof(box2));

            box1 = box1.GetRoundBox();
            box2 = box2.GetRoundBox();

            var v1 = box1.Min.X <= box2.Max.X;
            var v2 = box1.Max.X >= box2.Min.X;
            var v3 = box1.Min.Y <= box2.Max.Y;
            var v4 = box1.Max.Y >= box2.Min.Y;
            var v5 = box1.Min.Z <= box2.Max.Z;
            var v6 = box1.Max.Z >= box2.Min.Z;

            return v1 && v2 && v3 && v4 && v5 && v6;
        }

        /// <summary>
        ///     Gets the result of plane cross of the box.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="elm1"></param>
        /// <param name="elm2"></param>
        /// <returns></returns>
        public static bool CrossPlaneBox(this Document doc, Element elm1, Element elm2)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (elm1 is null)
                throw new ArgumentNullException(nameof(elm1));

            if (elm2 is null)
                throw new ArgumentNullException(nameof(elm2));

            var box1 = elm1.GetRoundBox(doc);
            var box2 = elm2.GetRoundBox(doc);
            var cp1 = (box1.Min - box2.Min).ToPlanePoint().CrossProduct((box2.Max - box2.Min).ToPlanePoint());
            var cp2 = (box2.Max - box2.Min).ToPlanePoint().CrossProduct((box1.Max - box2.Min).ToPlanePoint());
            var cp3 = (box2.Min - box1.Min).ToPlanePoint().CrossProduct((box1.Max - box1.Min).ToPlanePoint());
            var cp4 = (box1.Max - box1.Min).ToPlanePoint().CrossProduct((box2.Max - box1.Min).ToPlanePoint());
            var f1 = Math.Abs(cp1.DotProduct(cp2)) > 10e-3;
            var f2 = Math.Abs(cp3.DotProduct(cp4)) > 10e-3;

            return f1 && f2;
        }

        /// <summary>
        ///     Gets the result of plane cross of the box.
        /// </summary>
        /// <param name="box1"></param>
        /// <param name="box2"></param>
        /// <returns></returns>
        public static bool CrossPlaneBox(this BoundingBoxXYZ box1, BoundingBoxXYZ box2)
        {
            if (box1 is null)
                throw new ArgumentNullException(nameof(box1));

            if (box2 is null)
                throw new ArgumentNullException(nameof(box2));

            box1 = box1.GetRoundBox();
            box2 = box2.GetRoundBox();

            var cp1 = (box1.Min - box2.Min).ToPlanePoint().CrossProduct((box2.Max - box2.Min).ToPlanePoint());
            var cp2 = (box2.Max - box2.Min).ToPlanePoint().CrossProduct((box1.Max - box2.Min).ToPlanePoint());
            var cp3 = (box2.Min - box1.Min).ToPlanePoint().CrossProduct((box1.Max - box1.Min).ToPlanePoint());
            var cp4 = (box1.Max - box1.Min).ToPlanePoint().CrossProduct((box2.Max - box1.Min).ToPlanePoint());
            var f1 = Math.Abs(cp1.DotProduct(cp2)) > 10e-3;
            var f2 = Math.Abs(cp3.DotProduct(cp4)) > 10e-3;

            return f1 && f2;
        }

        /// <summary>
        ///     Gets the result of space cross of the box.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="elm1"></param>
        /// <param name="elm2"></param>
        /// <returns></returns>
        public static bool CrossSpaceBox(this Document doc, Element elm1, Element elm2)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (elm1 is null)
                throw new ArgumentNullException(nameof(elm1));

            if (elm2 is null)
                throw new ArgumentNullException(nameof(elm2));

            var box1 = elm1.GetRoundBox(doc);
            var box2 = elm2.GetRoundBox(doc);
            var cp1 = (box1.Min - box2.Min).CrossProduct(box2.Max - box2.Min);
            var cp2 = (box2.Max - box2.Min).CrossProduct(box1.Max - box2.Min);
            var cp3 = (box2.Min - box1.Min).CrossProduct(box1.Max - box1.Min);
            var cp4 = (box1.Max - box1.Min).CrossProduct(box2.Max - box1.Min);
            var f1 = Math.Abs(cp1.DotProduct(cp2)) > 10e-3;
            var f2 = Math.Abs(cp3.DotProduct(cp4)) > 10e-3;

            return f1 && f2;
        }

        /// <summary>
        ///     Gets the result of space cross of the box.
        /// </summary>
        /// <param name="box1"></param>
        /// <param name="box2"></param>
        /// <returns></returns>
        public static bool CrossSpaceBox(this BoundingBoxXYZ box1, BoundingBoxXYZ box2)
        {
            if (box1 is null)
                throw new ArgumentNullException(nameof(box1));

            if (box2 is null)
                throw new ArgumentNullException(nameof(box2));

            box1 = box1.GetRoundBox();
            box2 = box2.GetRoundBox();

            var cp1 = (box1.Min - box2.Min).CrossProduct(box2.Max - box2.Min);
            var cp2 = (box2.Max - box2.Min).CrossProduct(box1.Max - box2.Min);
            var cp3 = (box2.Min - box1.Min).CrossProduct(box1.Max - box1.Min);
            var cp4 = (box1.Max - box1.Min).CrossProduct(box2.Max - box1.Min);

            return cp1.DotProduct(cp2) > 10e-6 && cp3.DotProduct(cp4) > 10e-6;
        }

        /// <summary>
        ///     Gets the result of between the two elements can cut.
        /// </summary>
        /// <param name="elm1"></param>
        /// <param name="elm2"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public static bool CutElement(this Element elm1, Element elm2, CutFailureReason reason)
        {
            if (elm1 is null)
                throw new ArgumentNullException(nameof(elm1));

            if (elm2 is null)
                throw new ArgumentNullException(nameof(elm2));

            if (!Enum.IsDefined(typeof(CutFailureReason), reason))
                throw new InvalidEnumArgumentException(nameof(reason), (int) reason, typeof(CutFailureReason));

            return SolidSolidCutUtils.CanElementCutElement(elm1, elm2, out _);
        }

        /// <summary>
        ///     Gets the result of between the two elements cross.
        /// </summary>
        /// <param name="elm1"></param>
        /// <param name="elm2"></param>
        /// <returns></returns>
        public static bool CrossGeometry(this Element elm1, Element elm2)
        {
            if (elm1 is null)
                throw new ArgumentNullException(nameof(elm1));

            if (elm2 is null)
                throw new ArgumentNullException(nameof(elm2));

            return elm1.GetValidSolidList().CrossSolid(elm2.GetValidSolidList());
        }

        /// <summary>
        ///     Gets the result of between valid geometry of the two elements cross.
        /// </summary>
        /// <param name="ge1"></param>
        /// <param name="ge2"></param>
        /// <returns></returns>
        public static bool CrossGeometry(this GeometryElement ge1, GeometryElement ge2)
        {
            if (ge1 is null)
                throw new ArgumentNullException(nameof(ge1));

            if (ge2 is null)
                throw new ArgumentNullException(nameof(ge2));

            return ge1.GetValidSolidList().CrossSolid(ge2.GetValidSolidList());
        }

        /// <summary>
        ///     Gets the result of between some solids and others cross.
        /// </summary>
        /// <param name="solid1s"></param>
        /// <param name="solid2s"></param>
        /// <returns></returns>
        public static bool CrossSolid(this IEnumerable<Solid> solid1s, IEnumerable<Solid> solid2s)
        {
            if (solid1s is null)
                throw new ArgumentNullException(nameof(solid1s));

            if (solid2s is null)
                throw new ArgumentNullException(nameof(solid2s));

            return solid1s.FirstOrDefault(f => f.CrossSolid(solid2s)) != null;
        }

        /// <summary>
        ///     Gets the result of between a solid and some solids cross.
        /// </summary>
        /// <param name="solid"></param>
        /// <param name="solids"></param>
        /// <returns></returns>
        public static bool CrossSolid(this Solid solid, IEnumerable<Solid> solids)
        {
            if (solid is null)
                throw new ArgumentNullException(nameof(solid));

            if (solids is null)
                throw new ArgumentNullException(nameof(solids));

            return solids.FirstOrDefault(f => f.CrossSolid(solid)) != null;
        }

        /// <summary>
        ///     Gets the result of between a solid and a solid cross.
        /// </summary>
        /// <param name="solid1"></param>
        /// <param name="solid2"></param>
        /// <returns></returns>
        public static bool CrossSolid(this Solid solid1, Solid solid2)
        {
            if (solid1 is null)
                throw new ArgumentNullException(nameof(solid1));

            if (solid2 is null)
                throw new ArgumentNullException(nameof(solid2));

            return BooleanOperationsUtils.ExecuteBooleanOperation(solid1, solid2, BooleanOperationsType.Intersect)
                       .Volume > 0;
        }
    }
}