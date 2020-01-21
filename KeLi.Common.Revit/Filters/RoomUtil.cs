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

using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace KeLi.Common.Revit.Filters
{
    /// <summary>
    /// Room utility.
    /// </summary>
    public static class RoomUtil
    {
        /// <summary>
        /// Gets the room's edge list.
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static List<Line> GetEdgeList(this SpatialElement room)
        {
            var result = new List<Line>();
            var option = new SpatialElementBoundaryOptions
            {
                StoreFreeBoundaryFaces = true,
                SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.CoreBoundary
            };
            var segments = room.GetBoundarySegments(option).SelectMany(s => s);

            foreach (var seg in segments)
            {
                var sp = seg.GetCurve().GetEndPoint(0);
                var ep = seg.GetCurve().GetEndPoint(1);

                result.Add(Line.CreateBound(sp, ep));
            }

            return result;
        }

        /// <summary>
        /// Gets room list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="isValid"></param>
        /// <returns></returns>
        public static List<SpatialElement> GetSpatialElementList(this Document doc, bool isValid = true)
        {
            var results = doc.GetTypeElementList<SpatialElement>();

            if (isValid)
                results = results.Where(w => w?.Location != null && w.Area > 1e-6).ToList();

            return results;
        }

        /// <summary>
        /// Gets boundary wall list of the room.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<Wall> GetBoundaryWallList(this SpatialElement room, Document doc)
        {
            var results = new List<Wall>();
            var loops = room.GetBoundarySegments(new SpatialElementBoundaryOptions());

            foreach (var loop in loops)
            {
                foreach (var segment in loop)
                {
                    if (segment.ElementId.IntegerValue == -1)
                        continue;

                    if (doc.GetElement(segment.ElementId) is Wall wall)
                        results.Add(wall);
                }
            }

            return results;
        }
    }
}
