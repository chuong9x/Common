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
using KeLi.Common.Revit.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

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
        /// <param name="xStep"></param>
        /// <param name="yStep"></param>
        /// <param name="radius"></param>
        public static void DividePartList(Element elm, XYZ origin, XYZ baseX, double xStep, double yStep, double radius = 100000)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (origin is null)
                throw new ArgumentNullException(nameof(origin));

            if (baseX is null)
                throw new ArgumentNullException(nameof(baseX));

            if (xStep < 0)
                throw new ArgumentException(nameof(xStep));

            if (yStep < 0)
                throw new ArgumentException(nameof(yStep));

            if (radius < 0)
                throw new ArgumentException(nameof(radius));

            xStep = UnitConverter.Mm2Feet(xStep);
            yStep = UnitConverter.Mm2Feet(yStep);
            radius = UnitConverter.Mm2Feet(radius);

            var plane = Plane.CreateByNormalAndOrigin(XYZ.BasisZ, origin);
            var baseY = plane.Normal.CrossProduct(baseX);
            var lines = new List<Curve>();

            var xAxis = Line.CreateBound(origin - radius * baseX, origin + radius * baseX);
            var yAxis = Line.CreateBound(origin - radius * baseY, origin + radius * baseY);

            lines.Add(xAxis);
            lines.Add(yAxis);

            var yp0 = yAxis.GetEndPoint(0);
            var yp1 = yAxis.GetEndPoint(1);

            var xNum = Convert.ToInt32(Math.Ceiling(radius / xStep));
            var yNum = Convert.ToInt32(Math.Ceiling(radius / yStep));

            // Draws lines on x direction 
            for (var i = 0; i < xNum; i++)
            {
                var offset = (i + 1) * xStep * baseX;

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
                var offset = (i + 1) * yStep * baseY;

                // Above.
                lines.Add(Line.CreateBound(xp0 + offset, xp1 + offset));

                // Below.
                lines.Add(Line.CreateBound(xp0 - offset, xp1 - offset));
            }

            var doc = elm.Document;

            var sketchPlane = SketchPlane.Create(doc, plane);

            var ids = PartUtils.GetAssociatedParts(doc, elm.Id, true, true).ToList();

            if (ids.Count <= 0)
            {
                PartUtils.CreateParts(doc, new List<ElementId>() { elm.Id });

                doc.Regenerate();

                ids = PartUtils.GetAssociatedParts(doc, elm.Id, true, true).ToList();
            }

            if (ids.Count == 1)
                PartUtils.DivideParts(doc, ids, new List<ElementId>(), lines, sketchPlane.Id);
        }
    }
}
