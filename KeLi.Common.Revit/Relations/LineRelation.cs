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
using System.Linq;

using Autodesk.Revit.DB;

namespace KeLi.Common.Revit.Relations
{
    /// <summary>
    ///     About two lines relationship.
    /// </summary>
    public static class LineRelation
    {
        /// <summary>
        ///     Gets the result of whether the Line line1 and the Line line2 is space vertical.
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool IsSpaceVertical(this Line line1, Line line2, double tolerance = 2e-2)
        {
            if (line1 is null)
                throw new ArgumentNullException(nameof(line1));

            if (line2 is null)
                throw new ArgumentNullException(nameof(line2));

            return Math.Abs(line1.Direction.AngleTo(line2.Direction) - Math.PI / 2) < tolerance;
        }

        /// <summary>
        ///     Gets the result of whether the Line line1 and the Line line2 is space parallel.
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool IsSpaceParallel(this Line line1, Line line2, double tolerance = 2e-2)
        {
            if (line1 is null)
                throw new ArgumentNullException(nameof(line1));

            if (line2 is null)
                throw new ArgumentNullException(nameof(line2));

            if (Math.Abs(line1.Direction.AngleTo(line2.Direction) - Math.PI) < tolerance)
                return true;

            return line1.Direction.AngleTo(line2.Direction) < tolerance;
        }

        /// <summary>
        ///     Converts to plane line.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Line ToPlaneLine(this Line line)
        {
            if (line is null)
                throw new ArgumentNullException(nameof(line));

            var p1 = line.GetEndPoint(0);

            var p2 = line.GetEndPoint(1);

            if (p1.Z < p2.Z)
                return Line.CreateBound(new XYZ(p1.X, p1.Y, p1.Z), new XYZ(p2.X, p2.Y, p1.Z));

            return Line.CreateBound(new XYZ(p1.X, p1.Y, p2.Z), new XYZ(p2.X, p2.Y, p2.Z));
        }

        /// <summary>
        ///     Gets the result of whether the Line line1 and the Line line2 is plane parallel.
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool IsPlaneParallel(this Line line1, Line line2, double tolerance = 2e-2)
        {
            if (line1 is null)
                throw new ArgumentNullException(nameof(line1));

            if (line2 is null)
                throw new ArgumentNullException(nameof(line2));

            line1 = line1.ToPlaneLine();

            line2 = line2.ToPlaneLine();

            if (Math.Abs(line1.Direction.AngleTo(line2.Direction) - Math.PI) < tolerance)
                return true;

            return line1.Direction.AngleTo(line2.Direction) % Math.PI < tolerance;
        }

        /// <summary>
        ///     Gets the result of whether the Line line1 and the Line line2 is plane vertical.
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool IsPlaneVertical(this Line line1, Line line2, double tolerance = 2e-2)
        {
            if (line1 is null)
                throw new ArgumentNullException(nameof(line1));

            if (line2 is null)
                throw new ArgumentNullException(nameof(line2));

            line1 = line1.ToPlaneLine();

            line2 = line2.ToPlaneLine();

            return Math.Abs(line1.Direction.AngleTo(line2.Direction) - Math.PI / 2) < tolerance;
        }

        /// <summary>
        ///     Gets the intersection of the Line line1 and the Line line2 on plane.
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <param name="isTouch"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static XYZ GetPlaneCrossingPoint(this Line line1, Line line2, bool isTouch = true, double tolerance = 2e-2)
        {
            if (line1 is null)
                throw new ArgumentNullException(nameof(line1));

            if (line2 is null)
                throw new ArgumentNullException(nameof(line2));

            if (line1.IsPlaneParallel(line2))
                return null;

            XYZ result;

            var pt1 = line1.GetEndPoint(0);

            var pt2 = line1.GetEndPoint(1);

            var pt3 = line2.GetEndPoint(0);

            var pt4 = line2.GetEndPoint(1);

            var x1 = pt1.X;

            var y1 = pt1.Y;

            var x2 = pt2.X;

            var y2 = pt2.Y;

            var x3 = pt3.X;

            var y3 = pt3.Y;

            var x4 = pt4.X;

            var y4 = pt4.Y;

            var f1 = Math.Abs(line1.Direction.AngleTo(XYZ.BasisX) - Math.PI / 2) < tolerance;

            var f2 = Math.Abs(line2.Direction.AngleTo(XYZ.BasisX) - Math.PI / 2) < tolerance;

            // Must quadrature.
            if (line1.IsPlaneVertical(line2) && f1 || f2)
                result = f1 ? new XYZ(x1, y3, pt1.Z) : new XYZ(x3, y1, pt1.Z);

            else
            {
                var dx12 = x2 - x1;

                var dx31 = x3 - x1;

                var dx34 = x3 - x4;

                var dy21 = y2 - y1;

                var dy31 = y3 - y1;

                var dy34 = y3 - y4;

                var k1 = dx12 * dx34 * dy31 - x3 * dx12 * dy34 + x1 * dy21 * dx34;

                var k2 = dy21 * dx34 - dx12 * dy34;

                var k3 = dy21 * dy34 * dx31 - y3 * dy21 * dx34 + y1 * dx12 * dy34;

                var k4 = dx12 * dy34 - dy21 * dx34;

                // Equations of the state, by the formula to calculate the intersection.
                result = new XYZ(k1 / k2, k3 / k4, pt1.Z);
            }

            // It be used to calc the result in line1 and line2.
            var flag1 = (result.Y - x1) * (result.X - x2) <= 0;

            var flag2 = (result.Y - y1) * (result.Y - y2) <= 0;

            var flag3 = (result.X - x3) * (result.X - x4) <= 0;

            var flag4 = (result.Y - y3) * (result.Y - y4) <= 0;

            // No touch or true cross returns the intersection pt, otherwise returns null.
            return !isTouch || flag1 && flag2 && flag3 && flag4 ? result : null;
        }

        /// <summary>
        ///     Gets the intersections of the Line and the lines on plane.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="lines"></param>
        /// <param name="isTouch"></param>
        /// <returns></returns>
        public static List<XYZ> GetPlaneCrossingPointList(this Line line, IEnumerable<Line> lines, bool isTouch = true)
        {
            if (line is null)
                throw new ArgumentNullException(nameof(line));

            if (lines is null)
                throw new ArgumentNullException(nameof(lines));

            var results = new List<XYZ>();

            lines.ToList().ForEach(f => results.Add(line.GetPlaneCrossingPoint(f, isTouch)));

            return results.Where(w => w != null).ToList();
        }

        /// <summary>
        ///     Gets the distinct vectors of the Curve list.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static List<XYZ> GetDistinctPointList(this IEnumerable<Curve> curves)
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            var results = new List<XYZ>();

            foreach (var line in curves)
            {
                results.Add(line.GetEndPoint(0));

                results.Add(line.GetEndPoint(1));
            }

            for (var i = 0; i < results.Count; i++)
            {
                if (results[i] is null)
                    continue;

                for (var j = i + 1; j < results.Count; j++)
                {
                    if (results[j] is null)
                        continue;

                    if (results[i].IsAlmostEqualTo(results[j]))
                        results[j] = null;
                }
            }

            return results.Where(w => w != null).ToList();
        }

        /// <summary>
        ///     Gets points of the boundary.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static List<XYZ> GetBoundaryPointList(this IEnumerable<Curve> curves)
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            var results = new List<XYZ>();

            var tmpCurves = curves.ToList();

            foreach (var line in tmpCurves)
                results.Add(line.GetEndPoint(0));

            var endPoint = tmpCurves[tmpCurves.Count - 1].GetEndPoint(1);

            // If no closed, the last line's end point is different from the first line's start point.
            if (!tmpCurves[0].GetEndPoint(0).IsAlmostEqualTo(endPoint))
                results.Add(endPoint);

            return results;
        }

        /// <summary>
        ///     Gets the true point order by right(x), front(y) and top(z) point of the Curve list.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static XYZ GetMinPoint(this IEnumerable<Curve> curves)
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            return curves.Select(m => m.GetMinPoint()).GetMinPoint();
        }

        /// <summary>
        ///     Gets the true point order by top(z), front(y) and right(x) in the Curve list.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static XYZ GetMaxPoint(this IEnumerable<Curve> curves)
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            return curves.Select(m => m.GetMaxPoint()).GetMaxPoint();
        }

        /// <summary>
        ///     Gets the true point order by right(x), front(y) and top(z) point of the Curve.
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static XYZ GetMinPoint(this Curve curve)
        {
            if (curve is null)
                throw new ArgumentNullException(nameof(curve));

            return curve.GetEndPoints().GetMinPoint();
        }

        /// <summary>
        ///     Gets the true point order by top(z), front(y) and right(x) in the Curve.
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static XYZ GetMaxPoint(this Curve curve)
        {
            if (curve is null)
                throw new ArgumentNullException(nameof(curve));

            return curve.GetEndPoints().GetMaxPoint();
        }

        /// <summary>
        ///     Gets the two end points of the Curve.
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static List<XYZ> GetEndPoints(this Curve curve)
        {
            if (curve is null)
                throw new ArgumentNullException(nameof(curve));

            var pt1 = curve.GetEndPoint(0);

            var pt2 = curve.GetEndPoint(1);

            return new List<XYZ> { pt1, pt2 };
        }

        /// <summary>
        ///     Gets the true point order by top(z), front(y) and right(x) in the XYZ list.
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static XYZ GetMaxPoint(this IEnumerable<XYZ> pts)
        {
            if (pts is null)
                throw new ArgumentNullException(nameof(pts));

            return pts.OrderBy(o => o.X).ThenBy(o => o.Y).ThenBy(o => o.Z).FirstOrDefault();
        }

        /// <summary>
        ///     Gets the true point order by right(x), front(y) and top(z) in the XYZ list.
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static XYZ GetMinPoint(this IEnumerable<XYZ> pts)
        {
            if (pts is null)
                throw new ArgumentNullException(nameof(pts));

            return pts.OrderBy(o => o.Z).ThenBy(o => o.Y).ThenBy(o => o.X).FirstOrDefault();
        }

        /// <summary>
        ///     Gets the result of whether the point is in the plane direction polygon.
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static bool InPlanePolygon(this XYZ pt, IEnumerable<Line> polygon)
        {
            if (pt is null)
                throw new ArgumentNullException(nameof(pt));

            if (polygon is null)
                throw new ArgumentNullException(nameof(polygon));

            var x = pt.X;

            var y = pt.Y;

            var xs = new List<double>();

            var ys = new List<double>();

            foreach (var line in polygon)
            {
                xs.Add(line.GetEndPoint(0).X);

                ys.Add(line.GetEndPoint(0).Y);
            }

            var minX = xs.Min();

            var maxX = xs.Max();

            var minY = ys.Min();

            var maxY = ys.Max();

            var tmpPolygon = polygon.ToList();

            if (tmpPolygon.Count == 0 || x < minX || x > maxX || y < minY || y > maxY)
                return false;

            var result = false;

            for (int i = 0, j = tmpPolygon.Count - 1; i < tmpPolygon.Count; j = i++)
            {
                var dxji = xs[j] - xs[i];

                var dyji = ys[j] - ys[i];

                if (ys[i] > y != ys[j] > y && x < dxji * (y - ys[i]) / dyji + xs[i])
                    result = !result;
            }

            return result;
        }
    }
}