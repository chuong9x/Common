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
using KeLi.Common.Revit.Filters;
using KeLi.Common.Revit.Geometry;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    ///     ModelLine builder.
    /// </summary>
    public static class ModelCurveBuilder
    {
        /// <summary>
        /// Defines graphics style category's name of the ModelCurve for debug.
        /// </summary>
        private const string MODEL_COLOR = "DebugLine";

        /// <summary>
        ///     Creates a new ModelCurve.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pt"></param>
        /// <param name="sketchPlane"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static ModelCurve CreateModelCurve(this Document doc, XYZ pt, out SketchPlane sketchPlane, Color color = null)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (pt is null)
                throw new ArgumentNullException(nameof(pt));

            var line = Line.CreateBound(XYZ.Zero, pt);

            var result = doc.CreateModelCurve(line, out sketchPlane);

            SetModelCurveColor(result, doc, color);

            return result;
        }

        /// <summary>
        ///     Creates a new ModelCurve.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="sketchPlane"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static ModelCurve CreateModelCurve(this Document doc, XYZ pt1, XYZ pt2, out SketchPlane sketchPlane, Color color = null)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (pt1 is null)
                throw new ArgumentNullException(nameof(pt1));

            if (pt2 is null)
                throw new ArgumentNullException(nameof(pt2));

            var line = Line.CreateBound(pt1, pt2);

            var result = doc.CreateModelCurve(line, out sketchPlane);

            SetModelCurveColor(result, doc, color);

            return result;
        }

        /// <summary>
        ///     Creates a new ModelCurve.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="curve"></param>
        /// <param name="sketchPlane"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static ModelCurve CreateModelCurve(this Document doc, Curve curve, out SketchPlane sketchPlane, Color color = null)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (curve is null)
                throw new ArgumentNullException(nameof(curve));

            sketchPlane = null;

            XYZ normal = null;

            if (curve is Arc arc)
                normal = arc.Normal;

            if (curve is Ellipse ellipse)
                normal = ellipse.Normal;

            if (curve is Line line)
            {
                var refAsix = XYZ.BasisZ;

                if (line.IsSameDirection(XYZ.BasisZ, -XYZ.BasisZ))
                    refAsix = XYZ.BasisX;

                normal = line.Direction.CrossProduct(refAsix).Normalize();
            }

            if (normal == null)
                return null;

            var plane = Plane.CreateByNormalAndOrigin(normal, curve.GetEndPoint(0));

            sketchPlane = SketchPlane.Create(doc, plane);

            ModelCurve result;

            if (!doc.IsFamilyDocument)
                result = doc.Create.NewModelCurve(curve, sketchPlane);

            else
                result = doc.FamilyCreate.NewModelCurve(curve, sketchPlane);

            SetModelCurveColor(result, doc, color);

            return result;
        }

        /// <summary>
        ///     Creates ModelCurve list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pts"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static List<ModelCurve> CreateModelCurveList(this Document doc, IEnumerable<XYZ> pts, Color color = null)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (pts is null)
                throw new ArgumentNullException(nameof(pts));

            return pts.Select(s => doc.CreateModelCurve(s, out _, color)).ToList();
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

            return pts.Select(s => doc.CreateModelCurve(s, out _)).ToList();
        }

        /// <summary>
        ///     Creates ModelCurve list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="curves"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static List<ModelCurve> CreateModelCurveList(this Document doc, IEnumerable<Curve> curves, Color color = null)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            return curves.Select(s => doc.CreateModelCurve(s, out _, color)).ToList();
        }

        /// <summary>
        ///     Creates ModelCurve list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static List<ModelCurve> CreateModelCurveList(this Document doc, params Curve[] curves)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            return curves.Select(s => doc.CreateModelCurve(s, out _)).ToList();
        }

        /// <summary>
        ///     Set ModelCurve's graphics style color.
        /// </summary>
        /// <param name="modelCurve"></param>
        /// <param name="doc"></param>
        /// <param name="color"></param>
        private static void SetModelCurveColor(ModelCurve modelCurve, Document doc, Color color = null)
        {
            if (modelCurve is null)
                throw new ArgumentNullException(nameof(modelCurve));

            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            var lineCtg = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);

            if (!lineCtg.SubCategories.Contains(MODEL_COLOR))
            {
                var modelCtg = doc.Settings.Categories.NewSubcategory(lineCtg, MODEL_COLOR);

                if (color == null)
                    color = new Color(255, 0, 0);

                modelCtg.LineColor = color;
            }

            var graphicsStyles = doc.GetInstanceElementList<GraphicsStyle>();

            var modelStyle = graphicsStyles.FirstOrDefault(f => f.GraphicsStyleCategory.Name == MODEL_COLOR);

            var parm = modelCurve.get_Parameter(BuiltInParameter.BUILDING_CURVE_GSTYLE);

            if (modelStyle != null)
                parm.Set(modelStyle.Id);
        }
    }
}