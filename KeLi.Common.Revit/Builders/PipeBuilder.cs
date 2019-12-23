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
     |  |              Creation Time: 10/30/2019 07:08:41 PM |  |  |     |         |      |
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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Plumbing;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    /// Pipe builder.
    /// </summary>
    public static class PipeBuilder
    {
        /// <summary>
        /// Creates a new pipe.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="typeId"></param>
        /// <param name="lvlId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Pipe CreatePipe(this Document doc, ElementId typeId, ElementId lvlId, Connector start, Connector end)
        {
            return Pipe.Create(doc, typeId, lvlId, start, end);
        }

        /// <summary>
        /// Creates a new pipe.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="typeId"></param>
        /// <param name="lvlId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Pipe CreatePipe(this Document doc, ElementId typeId, ElementId lvlId, Connector start, XYZ end)
        {
            return Pipe.Create(doc, typeId, lvlId, start, end);
        }

        /// <summary>
        /// Creates a new pipe.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="systemId"></param>
        /// <param name="typeId"></param>
        /// <param name="lvlId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Pipe CreatePipe(this Document doc, ElementId systemId, ElementId typeId, ElementId lvlId, XYZ start,
            XYZ end)
        {
            return Pipe.Create(doc, systemId, typeId, lvlId, start, end);
        }

        /// <summary>
        /// Creates a new flex pipe.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="systemId"></param>
        /// <param name="typeId"></param>
        /// <param name="lvlId"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public static FlexPipe CreateFlexPipe(this Document doc, ElementId systemId, ElementId typeId, ElementId lvlId,
            List<XYZ> points)
        {
            return FlexPipe.Create(doc, systemId, typeId, lvlId, points);
        }

        /// <summary>
        /// Creates a new flex pipe.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="systemId"></param>
        /// <param name="typeId"></param>
        /// <param name="lvlId"></param>
        /// <param name="end"></param>
        /// <param name="points"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static FlexPipe CreateFlexPipe(this Document doc, ElementId systemId, ElementId typeId, ElementId lvlId,
            XYZ start, XYZ end, List<XYZ> points)
        {
            return FlexPipe.Create(doc, systemId, typeId, lvlId, start, end, points);
        }

        /// <summary>
        /// Creates a new conduit.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="typeId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="lvlId"></param>
        /// <returns></returns>
        public static Conduit CreateConduit(this Document doc, ElementId typeId, XYZ start, XYZ end, ElementId lvlId)
        {
            return Conduit.Create(doc, typeId, start, end, lvlId);
        }
    }
}