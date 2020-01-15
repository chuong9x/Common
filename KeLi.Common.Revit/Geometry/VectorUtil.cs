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
     |  |              Creation Time: 01/15/2020 03:11:11 PM |  |  |     |         |      |
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

namespace KeLi.Common.Revit.Geometry
{
    /// <summary>
    /// Vector utility.
    /// </summary>
    public static class VectorUtil
    {
        /// <summary>
        /// If true the specified line's direction and the specified direction are same.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static bool IsSameDirection(this Line line, XYZ direction)
        {
            return line.Direction.AngleTo(direction) < 1e-6;
        }

        /// <summary>
        /// If true the specified direction set and the line's direction are same.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="directions"></param>
        /// <returns></returns>
        public static bool IsSameDirection(this Line line, List<XYZ> directions)
        {
            return directions.Any(line.IsSameDirection);
        }

        /// <summary>
        /// If true the line1's direction and the line2's direction are same.
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="Line2"></param>
        /// <returns></returns>
        public static bool IsSameDirection(this Line line1, Line Line2)
        {
            return line1.Direction.AngleTo(Line2.Direction) < 1e-6;
        }

        /// <summary>
        /// If true the direction1 and the direction2 are same.
        /// </summary>
        /// <param name="direction1"></param>
        /// <param name="direction2"></param>
        /// <returns></returns>
        public static bool IsSameDirection(this XYZ direction1, XYZ direction2)
        {
            return direction1.AngleTo(direction2) < 1e-6;
        }
    }
}
