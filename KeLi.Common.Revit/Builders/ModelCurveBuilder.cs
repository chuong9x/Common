using System;
using System.Collections.Generic;
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
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<ModelCurve> CreateModelCurveList(this Document doc, List<Line> lines)
        {
            if (lines == null)
                throw new ArgumentNullException(nameof(lines));

            var results = new List<ModelCurve>();

            foreach (var line in lines)
                results.Add(doc.CreateModelCurve(line));

            return results;
        }
    }
}
