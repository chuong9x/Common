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

using static KeLi.Common.Revit.Properties.Resources;

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
        /// <param name="curve"></param>
        /// <param name="lineName"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static ModelCurve CreateModelCurve(this Curve curve, Document doc, string lineName = null, Color color = null)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (curve is null)
                throw new ArgumentNullException(nameof(curve));

            return doc.CreateModelCurve(curve, out _, lineName, color);
        }

        /// <summary>
        ///     Creates a new ModelCurve.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="curve"></param>
        /// <param name="lineName"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static ModelCurve CreateModelCurve(this Document doc, Curve curve, string lineName = null, Color color = null)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (curve is null)
                throw new ArgumentNullException(nameof(curve));

            return doc.CreateModelCurve(curve, out _, lineName, color);
        }

        /// <summary>
        ///     Creates a new ModelCurve.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="curve"></param>
        /// <param name="sketchPlane"></param>
        /// <param name="lineName"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static ModelCurve CreateModelCurve(this Curve curve, Document doc, out SketchPlane sketchPlane, string lineName = null, Color color = null)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (curve is null)
                throw new ArgumentNullException(nameof(curve));

            return doc.CreateModelCurve(curve, out sketchPlane, lineName, color);
        }

        /// <summary>
        ///     Creates a new ModelCurve.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="curve"></param>
        /// <param name="sketchPlane"></param>
        /// <param name="lineName"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static ModelCurve CreateModelCurve(this Document doc, Curve curve, out SketchPlane sketchPlane, string lineName = null, Color color = null)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (curve is null)
                throw new ArgumentNullException(nameof(curve));

            XYZ normal = null;
            XYZ endPt = null;

            if (curve is Arc arc)
            {
                normal = arc.Normal;
                endPt = arc.Center;
            }

            else if (curve is Ellipse ellipse)
            {
                normal = ellipse.Normal;
                endPt = ellipse.Center;
            }

            else if (curve is Line line)
            {
                var refAsix = XYZ.BasisZ;

                if (Math.Abs(line.Direction.AngleTo(XYZ.BasisZ)) < 1e-6)
                    refAsix = XYZ.BasisX;

                else if (Math.Abs(line.Direction.AngleTo(-XYZ.BasisZ)) < 1e-6)
                    refAsix = XYZ.BasisX;

                normal = line.Direction.CrossProduct(refAsix).Normalize();
                endPt = line.Origin;
            }

            if (normal == null)
                throw new NullReferenceException(nameof(normal));

            var plane = normal.CreatePlane(endPt);

            sketchPlane = SketchPlane.Create(doc, plane);

            var result = !doc.IsFamilyDocument ? doc.Create.NewModelCurve(curve, sketchPlane) 
                : doc.FamilyCreate.NewModelCurve(curve, sketchPlane);

            result.SetModelCurveColor(lineName, color);

            return result;
        }

        /// <summary>
        ///     Creates ModelCurve list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="curves"></param>
        /// <param name="lineName"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static List<ModelCurve> CreateModelCurveList(this Document doc, IEnumerable<Curve> curves, string lineName = null, Color color = null)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            return curves.Select(s => doc.CreateModelCurve(s, out _, lineName, color)).ToList();
        }

        /// <summary>
        ///     Creates ModelCurve list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="lineName"></param>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static List<ModelCurve> CreateModelCurveList(this Document doc, string lineName = null, params Curve[] curves)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            return curves.Select(s => doc.CreateModelCurve(s, out _, lineName)).ToList();
        }

        /// <summary>
        ///     Set ModelCurve's graphics style color.
        /// </summary>
        /// <param name="modelCurve"></param>
        /// <param name="lineName"></param>
        /// <param name="color"></param>
        public static void SetModelCurveColor(this ModelCurve modelCurve, string lineName = null, Color color = null)
        {
            if (modelCurve is null)
                throw new ArgumentNullException(nameof(modelCurve));

            var doc = modelCurve.Document;
            var lineCtg = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);

            if (string.IsNullOrWhiteSpace(lineName))
                lineName = DebugLine;

            if (!lineCtg.SubCategories.Contains(lineName))
            {
                if (color == null)
                    color = new Color(255, 0, 0);

                var modelCtg = doc.Settings.Categories.NewSubcategory(lineCtg, lineName);

                modelCtg.LineColor = color;
            }

            var graphicsStyles = doc.GetInstanceList<GraphicsStyle>();
            var modelStyle = graphicsStyles.FirstOrDefault(f => f.GraphicsStyleCategory.Name == lineName);
            var parm = modelCurve.get_Parameter(BuiltInParameter.BUILDING_CURVE_GSTYLE);

            if (modelStyle != null)
                parm.Set(modelStyle.Id);
        }
    }
}