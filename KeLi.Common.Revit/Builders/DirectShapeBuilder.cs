using System.Collections.Generic;
using Autodesk.Revit.DB;
using KeLi.Common.Revit.Entities;
using static Autodesk.Revit.DB.GeometryCreationUtilities;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    /// DirectShape builder.
    /// </summary>
    public class DirectShapeBuilder
    {
        /// <summary>
        /// Creates DirectShape.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="parm"></param>
        /// <param name="opt"></param>
        /// <returns></returns>
        public static DirectShape CreateDirectShape(Document doc, ExtrusionParm parm, SolidOptions opt = null)
        {
            var solid = CreateExtrusionGeometry(parm.Profile, parm.Direction, parm.Distance);

            if (opt != null)
                solid = CreateExtrusionGeometry(parm.Profile, parm.Direction, parm.Distance, opt);

            var result = DirectShape.CreateElement(doc, new ElementId(parm.Category));

            result?.AppendShape(new List<GeometryObject> { solid });

            return result;
        }
    }
}
