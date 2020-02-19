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
     |  |              Creation Time: 12/27/2019 07:13:20 PM |  |  |     |         |      |
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
using KeLi.Common.Revit.Geometry;

namespace KeLi.Common.Revit.Filters
{
    /// <summary>
    ///     Room utility.
    /// </summary>
    public static class RoomUtil
    {
        /// <summary>
        ///     Gets the room's edge list.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="boundary"></param>
        /// <returns></returns>
        public static List<Line> GetEdgeLineList(this SpatialElement room, SpatialElementBoundaryLocation boundary)
        {
            if (room is null)
                throw new ArgumentNullException(nameof(room));

            var result = new List<Line>();
            var opt = new SpatialElementBoundaryOptions
            {
                StoreFreeBoundaryFaces = true,
                SpatialElementBoundaryLocation = boundary
            };
            var segs = room.GetBoundarySegments(opt).SelectMany(s => s);

            foreach (var seg in segs)
            {
                var sp = seg.GetCurve().GetEndPoint(0);
                var ep = seg.GetCurve().GetEndPoint(1);

                result.Add(Line.CreateBound(sp, ep));
            }

            return result;
        }

        /// <summary>
        ///     Gets boundary wall list of the room.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="doc"></param>
        /// <param name="maxThickness"></param>
        /// <returns></returns>
        public static List<Wall> GetBoundaryWallList(this SpatialElement room, Document doc, double maxThickness = 80)
        {
            if (room is null)
                throw new ArgumentNullException(nameof(room));

            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            const BuiltInParameter parmEnum = BuiltInParameter.WALL_ATTR_WIDTH_PARAM;
            var results = new List<Wall>();
            var loops = room.GetBoundarySegments(new SpatialElementBoundaryOptions());

            foreach (var loop in loops)
            foreach (var segment in loop)
            {
                // It's invalid!
                if (segment.ElementId.IntegerValue == -1)
                    continue;

                // Because base room boundary to do, so one wall maybe be picked up some times.
                if (results.FirstOrDefault(f => f.Id == segment.ElementId) != null)
                    continue;

                if (doc.GetElement(segment.ElementId) is Wall wall)
                    results.Add(wall);
            }

            return results.Where(w => Convert.ToDouble(w.WallType.get_Parameter(parmEnum).AsValueString()) < maxThickness).ToList();
        }

        /// <summary>
        ///     Gets inner face of wall.
        /// </summary>
        /// <param name="wall"></param>
        /// <param name="refPt"></param>
        /// <returns></returns>
        public static Face GetInnerFace(this Wall wall, XYZ refPt)
        {
            if (wall is null)
                throw new ArgumentNullException(nameof(wall));

            if (refPt is null)
                throw new ArgumentNullException(nameof(refPt));

            var line = wall.GetLocationCurve() as Line;

            if (line is null)
                throw new Exception("Curve wall isn't supported!");

            var wdir = GetLineDirection(line, refPt);
            var innerNormal = GetInnerNormal(wdir);

            return wall.GetFaceList(innerNormal).FirstOrDefault();
        }

        /// <summary>
        ///     Gets inner direction noraml.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static XYZ GetInnerNormal(this LineDirection dir)
        {
            switch (dir)
            {
                case LineDirection.East:
                    return -XYZ.BasisX;
                case LineDirection.West:
                    return XYZ.BasisX;
                case LineDirection.South:
                    return XYZ.BasisY;
                case LineDirection.North:
                    return -XYZ.BasisY;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
            }
        }

        /// <summary>
        ///     Gets direction of the line by a reference specified point.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="refPt"></param>
        /// <returns></returns>
        public static LineDirection GetLineDirection(this Line line, XYZ refPt)
        {
            if (line is null)
                throw new ArgumentNullException(nameof(line));

            if (refPt is null)
                throw new ArgumentNullException(nameof(refPt));

            // X axis direction.
            if (Math.Abs(line.Direction.Y) < 1e-6)
            {
                // South
                if (line.Origin.Y < refPt.Y)
                    return LineDirection.South;

                // North
                return LineDirection.North;
            }

            // Y axis direction.
            if (Math.Abs(line.Direction.X) < 1e-6)
            {
                // West
                if (line.Origin.X < refPt.X)
                    return LineDirection.West;

                // East
                return LineDirection.East;
            }

            throw new Exception("The wall location's direction isn't supported!");
        }
    }
}