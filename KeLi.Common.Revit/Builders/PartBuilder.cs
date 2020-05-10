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
     |  |              Creation Time: 04/29/2020 03:48:41 PM |  |  |     |         |      |
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

using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB.Visual;

using static Autodesk.Revit.DB.DisplayUnitType;
using static Autodesk.Revit.DB.Visual.UnifiedBitmap;

using static KeLi.Common.Revit.Builders.PartBuilder.TextureAngle;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    ///     Part builder.
    /// </summary>
    public static class PartBuilder
    {
        /// <summary>
        /// Divide part list.
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="origin"></param>
        /// <param name="baseX"></param>
        /// <param name="step"></param>
        /// <param name="initAngle"></param>
        /// <param name="radius"></param>
        public static void DividePartList(Element elm, XYZ origin, XYZ baseX, double[] step, TextureAngle initAngle, double radius = 5000)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (origin is null)
                throw new ArgumentNullException(nameof(origin));

            if (baseX is null)
                throw new ArgumentNullException(nameof(baseX));

            if (step[0] < 0)
                throw new ArgumentException(nameof(step));

            if (step[1] < 0)
                throw new ArgumentException(nameof(step));

            if (radius < 0)
                throw new ArgumentException(nameof(radius));

            if (initAngle == Rotation90 || initAngle == Rotation270)
                step = new[] { step[1], step[0] };

            var plane = Plane.CreateByNormalAndOrigin(XYZ.BasisZ, origin);
            var baseY = plane.Normal.CrossProduct(baseX);
            var lines = new List<Curve>();

            var xAxis = Line.CreateBound(origin - radius * baseX, origin + radius * baseX);
            var yAxis = Line.CreateBound(origin - radius * baseY, origin + radius * baseY);

            lines.Add(xAxis);
            lines.Add(yAxis);

            var yp0 = yAxis.GetEndPoint(0);
            var yp1 = yAxis.GetEndPoint(1);

            var xNum = Convert.ToInt32(Math.Ceiling(radius / step[0]));
            var yNum = Convert.ToInt32(Math.Ceiling(radius / step[1]));

            // Draws lines on x direction 
            for (var i = 0; i < xNum; i++)
            {
                var offset = (i + 1) * step[0] * baseX;

                // On Right.
                lines.Add(Line.CreateBound(yp0 + offset, yp1 + offset));

                // On Left.
                lines.Add(Line.CreateBound(yp0 - offset, yp1 - offset));
            }

            var xp0 = xAxis.GetEndPoint(0);
            var xp1 = xAxis.GetEndPoint(1);

            // Draws lines on y direction 
            for (var i = 0; i < yNum; i++)
            {
                var offset = (i + 1) * step[1] * baseY;

                // Above.
                lines.Add(Line.CreateBound(xp0 + offset, xp1 + offset));

                // Below.
                lines.Add(Line.CreateBound(xp0 - offset, xp1 - offset));
            }

            var doc = elm.Document;
            var sketchPlane = SketchPlane.Create(doc, plane);
            var ids = PartUtils.GetAssociatedParts(doc, elm.Id, true, true).ToList();

            foreach (var id in ids)
            {
                if (doc.GetElement(id) == null)
                    continue;

                var result = doc.GetElement(id).IsValidObject;

                if (!result)
                    continue;

                doc.Delete(PartUtils.GetAssociatedPartMaker(doc, id).Id);
            }

            if (ids.Count <= 0)
            {
                PartUtils.CreateParts(doc, new List<ElementId> { elm.Id });

                doc.Regenerate();

                ids = PartUtils.GetAssociatedParts(doc, elm.Id, true, true).ToList();
            }

            if (ids.Count == 1)
                PartUtils.DivideParts(doc, ids, new List<ElementId>(), lines, sketchPlane.Id);
        }

        /// <summary>
        ///     Sets texture.
        /// </summary>
        /// <param name="material"></param>
        /// <param name="angle"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="initAngle"></param>
        public static void SetTexture(Material material, double angle, double[] offset, double[] size, TextureAngle initAngle)
        {
            var doc = material.Document;

            if (offset == null || offset.Length == 0)
                return;

            if (size == null || size.Length == 0)
                return;

            if (initAngle == Rotation90 || initAngle == Rotation270)
                size = new[] { size[1], size[0] };

            if (initAngle == Rotation90 || initAngle == Rotation270)
                offset = new[] { offset[1], offset[0] };

            angle += (int)initAngle;

            angle %= 360;

            using (var editScope = new AppearanceAssetEditScope(doc))
            {
                var asset = editScope.Start(material.AppearanceAssetId);

                if (angle >= 0 && angle <= 360)
                {
                    var angleProp = (AssetPropertyDouble)asset[TextureWAngle];

                    if (angleProp != null)
                        angleProp.Value = angle;
                }

                if (asset[TextureRealWorldOffsetX] is AssetPropertyDistance xOffsetProp)
                    xOffsetProp.Value = UnitUtils.Convert(offset[0], DUT_DECIMAL_FEET, xOffsetProp.DisplayUnitType);

                if (asset[TextureRealWorldOffsetY] is AssetPropertyDistance yOffsetProp)
                    yOffsetProp.Value = UnitUtils.Convert(offset[1], DUT_DECIMAL_FEET, yOffsetProp.DisplayUnitType);

                var xSizeProp = (AssetPropertyDistance)asset[TextureRealWorldScaleX];

                if (xSizeProp != null)
                {
                    var minXSize = UnitUtils.Convert(0.01, xSizeProp.DisplayUnitType, DUT_MILLIMETERS);

                    if (size[0] > minXSize)
                        xSizeProp.Value = UnitUtils.Convert(size[0], DUT_DECIMAL_FEET, xSizeProp.DisplayUnitType);
                }

                var ySizeProp = (AssetPropertyDistance)asset[TextureRealWorldScaleY];

                if (ySizeProp != null)
                {
                    var minYSize = UnitUtils.Convert(0.01, ySizeProp.DisplayUnitType, DUT_MILLIMETERS);

                    if (size[1] > minYSize)
                        ySizeProp.Value = UnitUtils.Convert(size[1], DUT_DECIMAL_FEET, ySizeProp.DisplayUnitType);
                }

                editScope.Commit(true);
            }
        }

        /// <summary>
        ///     Computes texture's initial angle.
        /// </summary>
        /// <param name="baseX"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static double ComputeTextureAngle(XYZ baseX, double tolerance = 1e-9)
        {
            var baseRadian = baseX.AngleTo(XYZ.BasisX);

            // To compute min radian with x asix.
            if (baseRadian >= Math.PI / 2)
                baseRadian = Math.PI - baseRadian;

            var result = 0d;

            // 1st quadrant.
            if (baseX.X > tolerance && baseX.Y > tolerance)
                result = 90 - baseRadian * 180 / Math.PI;

            // 2nd quadrant.
            else if (baseX.X < -tolerance && baseX.Y > tolerance)
                result = 270 + baseRadian * 180 / Math.PI;

            // 3rd quadrant.
            else if (baseX.X < -tolerance && baseX.Y < -tolerance)
                result = 180 + baseRadian * 180 / Math.PI;

            // 4th quadrant.
            else if (baseX.X > tolerance && baseX.Y < -tolerance)
                result = 90 + baseRadian * 180 / Math.PI;

            // →
            else if (baseX.X > tolerance && baseX.Y <= tolerance)
                result = 90;

            // ↓
            else if (baseX.X <= tolerance && baseX.Y < -tolerance)
                result = 180;

            // ←
            else if (baseX.X < -tolerance && baseX.Y <= tolerance)
                result = 270;

            // ↑ 
            else if (baseX.X > tolerance && baseX.Y <= tolerance)
                result = 0;

            return result;
        }

        /// <summary>
        ///     Texture angle.
        /// </summary>
        public enum TextureAngle
        {
            /// <summary>
            ///     Initializes angle.
            /// </summary>
            Rotation0 = 0,

            /// <summary>
            ///     Rotates 90 angle.
            /// </summary>
            Rotation90 = 90,

            /// <summary>
            ///     Rotates 180 angle.
            /// </summary>
            Rotation180 = 180,

            /// <summary>
            ///     Rotates 270 angle.
            /// </summary>
            Rotation270 = 270,
        }
    }
}
