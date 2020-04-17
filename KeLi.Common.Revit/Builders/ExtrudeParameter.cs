using System;

using Autodesk.Revit.DB;

using KeLi.Common.Revit.Converters;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    ///     Extrude parameter
    /// </summary>
    public class ExtrudeParameter
    {
        /// <summary>
        ///     Extrude parameter
        /// </summary>
        /// <param name="boundary"></param>
        /// <param name="plane"></param>
        /// <param name="thick"></param>
        /// <param name="convertUnit"></param>
        public ExtrudeParameter(CurveArrArray boundary, Plane plane, double thick, bool convertUnit = true)
        {
            Boundary = boundary ?? throw new ArgumentNullException(nameof(boundary));

            Plane = plane ?? throw new ArgumentNullException(nameof(plane));

            Thick = convertUnit ? UnitConverter.ConvertMmToFeet(thick) : thick;
        }

        /// <summary>
        ///     The Extrude's boundary.
        /// </summary>
        public CurveArrArray Boundary { get; }

        /// <summary>
        ///     The extrude's reference plane.
        /// </summary>
        public Plane Plane { get; }

        /// <summary>
        ///     The Extrude's thick.
        /// </summary>
        public double Thick { get; }
    }
}