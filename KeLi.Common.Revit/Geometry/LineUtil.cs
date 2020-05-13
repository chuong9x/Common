using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KeLi.Common.Revit.Geometry
{
    /// <summary>
    ///     Line utiltity.
    /// </summary>
    public static class LineUtil
    {
        /// <summary>
        ///     Gets the round point with custom precision.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static XYZ GetRoundPoint(this XYZ point, int precision = 4)
        {
            if (point is null)
                throw new ArgumentNullException(nameof(point));

            var roundX = Math.Round(point.X, precision);
            var roundY = Math.Round(point.Y, precision);
            var roundZ = Math.Round(point.Z, precision);

            return new XYZ(roundX, roundY, roundZ);
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

    }
}
