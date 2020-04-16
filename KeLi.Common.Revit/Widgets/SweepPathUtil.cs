using Autodesk.Revit.DB;

namespace KeLi.Common.Revit.Widgets
{
    /// <summary>
    ///     Sweep path utility.
    /// </summary>
    public static class SweepPathUtil
    {
        /// <summary>
        ///     Computes the profile's plane.
        /// </summary>
        /// <param name="loftingPath"></param>
        /// <param name="fdoc"></param>
        /// <returns></returns>
        public static SketchPlane ComputeProfilePlane(this CurveArray loftingPath, Document fdoc)
        {
            var curve1 = loftingPath.get_Item(0);

            var curve2 = loftingPath.get_Item(1);

            var p0 = curve1.GetEndPoint(0);

            var p1 = curve1.GetEndPoint(1);

            var p2 = curve2.GetEndPoint(1);

            var plane = Plane.CreateByThreePoints(p0, p1, p2);

            return SketchPlane.Create(fdoc, plane);
        }
    }
}