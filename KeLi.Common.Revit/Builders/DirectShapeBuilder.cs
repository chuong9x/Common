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
     |  |              Creation Time: 01/15/2020 10:22:11 AM |  |  |     |         |      |
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

using static Autodesk.Revit.DB.GeometryCreationUtilities;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    ///     DirectShape builder.
    /// </summary>
    public static class DirectShapeBuilder
    {
        /// <summary>
        ///     Creates a new DirectShape.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="parm"></param>
        /// <param name="opt"></param>
        /// <returns></returns>
        public static DirectShape CreateDirectShape(this Document doc, DirectShapeParameter parm, SolidOptions opt = null)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            var solid = CreateExtrusionGeometry(parm.Profile.ToList(), parm.Direction, parm.Distance);

            if (opt != null)
                solid = CreateExtrusionGeometry(parm.Profile.ToList(), parm.Direction, parm.Distance, opt);

            DirectShape result;

            #if R2016

            var appGuid = Guid.NewGuid().ToString();

            var dataGuid = Guid.NewGuid().ToString();

            // TODO: It's should test.
            result = DirectShape.CreateElement(doc, new ElementId(parm.Category), appGuid, dataGuid);

            #endif

            #if R2018
            
            result = DirectShape.CreateElement(doc, new ElementId(parm.Category));
            
            #endif

            result?.AppendShape(new List<GeometryObject> { solid });

            return result;
        }
    }
}