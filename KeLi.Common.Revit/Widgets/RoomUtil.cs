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

using Option = Autodesk.Revit.DB.SpatialElementBoundaryOptions;

namespace KeLi.Common.Revit.Widgets
{
    /// <summary>
    ///     Room utility.
    /// </summary>
    public static class RoomUtil
    {
        /// <summary>
        ///     Gets boundary wall list of the room.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="doc"></param>
        /// <param name="opt"></param>
        /// <param name="maxThickness"></param>
        /// <returns></returns>
        public static List<Wall> GetBoundaryWallList(this SpatialElement room, Document doc, Option opt = null, double maxThickness = 80)
        {
            if (room is null)
                throw new ArgumentNullException(nameof(room));

            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            var results = new List<Wall>();

            var segments = room.GetBoundarySegments(opt).SelectMany(s => s);

            foreach (var segment in segments)
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

            const BuiltInParameter parmEnum = BuiltInParameter.WALL_ATTR_WIDTH_PARAM;

            return results.Where(w => Convert.ToDouble(w.WallType.get_Parameter(parmEnum).AsValueString()) < maxThickness).ToList();
        }
    }
}