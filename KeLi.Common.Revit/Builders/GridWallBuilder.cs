using Autodesk.Revit.DB;

using KeLi.Common.Revit.Filters;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    /// Grid wall builder.
    /// </summary>
    public static class GridWallBuilder
    {
        /// <summary>
        ///     Creates a new grid wall.
        /// </summary>
        /// <param name="wall"></param>
        /// <param name="room"></param>
        /// <param name="view"></param>
        public static void CreateGridWall(this Wall wall, SpatialElement room, View3D view)
        {
            var doc = wall.Document;

            var plane = wall.GetNearestPlanarFace(room, view);
        }
    }
}
