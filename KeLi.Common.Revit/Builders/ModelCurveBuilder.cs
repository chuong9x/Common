using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using KeLi.Common.Revit.Geometry;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    /// ModelLine builder.
    /// </summary>
    public static class ModelCurveBuilder
    {
        /// <summary>
        /// Creates a new ModelCurve.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static ModelCurve CreateModelCurve(this Document doc, XYZ pt)
        {
            if (pt == null)
                throw new ArgumentNullException(nameof(pt));

            var line = Line.CreateBound(XYZ.Zero, pt);

            return doc.CreateModelCurve(line);
        }

        /// <summary>
        /// Creates a new ModelCurve.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static ModelCurve CreateModelCurve(this Document doc, XYZ pt1, XYZ pt2)
        {
            if (pt1 == null)
                throw new ArgumentNullException(nameof(pt1));

            var line = Line.CreateBound(pt1, pt2);

            return doc.CreateModelCurve(line);
        }

        /// <summary>
        /// Creates a new ModelCurve.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static ModelCurve CreateModelCurve(this Document doc, Line line)
        {
            if (line == null)
                throw new ArgumentNullException(nameof(line));

            var basicZ = XYZ.BasisZ;

            if (line.IsSameDirection(new List<XYZ> { XYZ.BasisZ, -XYZ.BasisZ }))
                basicZ = XYZ.BasisY;

            var normal = basicZ.CrossProduct(line.Direction).Normalize();
            var plane = Plane.CreateByNormalAndOrigin(normal, line.GetEndPoint(0));
            var sketchPlane = SketchPlane.Create(doc, plane);

            return doc.Create.NewModelCurve(line, sketchPlane);
        }

        /// <summary>
        /// Creates ModelCurve set.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static List<ModelCurve> CreateModelCurveList(this Document doc, List<XYZ> pts)
        {
            if (pts == null)
                throw new ArgumentNullException(nameof(pts));

            return pts.Select(doc.CreateModelCurve).ToList();
        }

        /// <summary>
        /// Creates ModelCurve set.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static List<ModelCurve> CreateModelCurveList(this Document doc, params XYZ[] pts)
        {
            if (pts == null)
                throw new ArgumentNullException(nameof(pts));

            return pts.Select(doc.CreateModelCurve).ToList();
        }

        /// <summary>
        /// Creates ModelCurve set.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<ModelCurve> CreateModelCurveList(this Document doc, List<Line> lines)
        {
            if (lines == null)
                throw new ArgumentNullException(nameof(lines));

            return lines.Select(doc.CreateModelCurve).ToList();
        }

        /// <summary>
        /// Creates ModelCurve set.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<ModelCurve> CreateModelCurveList(this Document doc, params Line[] lines)
        {
            if (lines == null)
                throw new ArgumentNullException(nameof(lines));

            return lines.Select(doc.CreateModelCurve).ToList();
        }
    }
}
