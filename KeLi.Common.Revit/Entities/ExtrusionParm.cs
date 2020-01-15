using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace KeLi.Common.Revit.Entities
{
    /// <summary>
    /// Extrusion paramter.
    /// </summary>
    public class ExtrusionParm
    {
        /// <summary>
        /// Extrusion paramter.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="direction"></param>
        /// <param name="distance"></param>
        /// <param name="category"></param>
        public ExtrusionParm(List<CurveLoop> profile, XYZ direction, double distance, BuiltInCategory category)
        {
            Profile = profile;
            Direction = direction;
            Distance = distance;
            Category = category;
        }

        /// <summary>
        /// Extrusion's profile.
        /// </summary>
        public List<CurveLoop> Profile { get; set; }

        /// <summary>
        /// Extrusion's direction.
        /// </summary>
        public XYZ  Direction { get; set; }

        /// <summary>
        /// Extrusion's distance.
        /// </summary>
        public double  Distance { get; set; }

        /// <summary>
        /// Extrusion's category.
        /// </summary>
        public BuiltInCategory Category { get; set; }
    }
}