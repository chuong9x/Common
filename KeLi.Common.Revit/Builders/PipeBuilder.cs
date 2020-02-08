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

using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Plumbing;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    ///     Pipe builder.
    /// </summary>
    public static class PipeBuilder
    {
        /// <summary>
        ///     Creates a new pipe.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="parm"></param>
        /// <param name="startConn"></param>
        /// <param name="endConn"></param>
        /// <returns></returns>
        public static Pipe CreatePipe(this Document doc, PipeParameter parm, Connector startConn, Connector endConn)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            if (startConn is null)
                throw new ArgumentNullException(nameof(startConn));

            if (endConn is null)
                throw new ArgumentNullException(nameof(endConn));

            return Pipe.Create(doc, parm.TypeId, parm.LevelId, startConn, endConn);
        }

        /// <summary>
        ///     Creates a new pipe.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="parm"></param>
        /// <param name="startConn"></param>
        /// <param name="endPt"></param>
        /// <returns></returns>
        public static Pipe CreatePipe(this Document doc, PipeParameter parm, Connector startConn, XYZ endPt)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            if (startConn is null)
                throw new ArgumentNullException(nameof(startConn));

            if (endPt is null)
                throw new ArgumentNullException(nameof(endPt));

            return Pipe.Create(doc, parm.TypeId, parm.LevelId, startConn, endPt);
        }

        /// <summary>
        ///     Creates a new pipe.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="parm"></param>
        /// <param name="startPt"></param>
        /// <param name="endPt"></param>
        /// <returns></returns>
        public static Pipe CreatePipe(this Document doc, PipeParameter parm, XYZ startPt, XYZ endPt)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            if (startPt is null)
                throw new ArgumentNullException(nameof(startPt));

            if (endPt is null)
                throw new ArgumentNullException(nameof(endPt));

            return Pipe.Create(doc, parm.SystemId, parm.TypeId, parm.LevelId, startPt, endPt);
        }

        /// <summary>
        ///     Creates a new flex pipe.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="parm"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public static FlexPipe CreateFlexPipe(this Document doc, PipeParameter parm, IEnumerable<XYZ> points)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            if (points is null)
                throw new ArgumentNullException(nameof(points));

            return FlexPipe.Create(doc, parm.SystemId, parm.TypeId, parm.LevelId, points.ToList());
        }

        /// <summary>
        ///     Creates a new flex pipe.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="parm"></param>
        /// <param name="startPt"></param>
        /// <param name="endPt"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public static FlexPipe CreateFlexPipe(this Document doc, PipeParameter parm, XYZ startPt, XYZ endPt,
            IEnumerable<XYZ> points)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            if (startPt is null)
                throw new ArgumentNullException(nameof(startPt));

            if (endPt is null)
                throw new ArgumentNullException(nameof(endPt));

            if (points is null)
                throw new ArgumentNullException(nameof(points));

            return FlexPipe.Create(doc, parm.SystemId, parm.TypeId, parm.LevelId, startPt, endPt, points.ToList());
        }

        /// <summary>
        ///     Creates a new conduit.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="parm"></param>
        /// <param name="startPt"></param>
        /// <param name="endPt"></param>
        /// <returns></returns>
        public static Conduit CreateConduit(this Document doc, PipeParameter parm, XYZ startPt, XYZ endPt)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            if (startPt is null)
                throw new ArgumentNullException(nameof(startPt));

            if (endPt is null)
                throw new ArgumentNullException(nameof(endPt));

            return Conduit.Create(doc, parm.TypeId, startPt, endPt, parm.LevelId);
        }
    }
}