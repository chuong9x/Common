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
     |  |              Creation Time: 01/21/2020 06:53:11 PM |  |  |     |         |      |
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

using Autodesk.Revit.DB;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    ///     Curtain system parameter.
    /// </summary>
    public class CurtainSystemParameter
    {
        /// <summary>
        ///     Curtain system parameter for ceiling, floor, etc.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="typeName"></param>
        /// <param name="pnlType"></param>
        /// <param name="tplFileName"></param>
        public CurtainSystemParameter(SpatialElement room, string typeName, PanelType pnlType, string tplFileName)
        {
            Room = room ?? throw new ArgumentNullException(nameof(room));

            TypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));

            PanelType = pnlType ?? throw new ArgumentNullException(nameof(pnlType));

            TemplateFileName = tplFileName ?? throw new ArgumentNullException(nameof(tplFileName));
        }

        /// <summary>
        ///     Curtain system parameter for wall.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="wall"></param>
        /// <param name="typeName"></param>
        /// <param name="pnlType"></param>
        /// <param name="tplName"></param>
        public CurtainSystemParameter(SpatialElement room, Wall wall, string typeName, PanelType pnlType, string tplName)
        {
            ReferenceWall = wall ?? throw new ArgumentNullException(nameof(wall));

            Room = room ?? throw new ArgumentNullException(nameof(room));

            TypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));

            PanelType = pnlType ?? throw new ArgumentNullException(nameof(pnlType));

            TemplateFileName = tplName ?? throw new ArgumentNullException(nameof(tplName));
        }

        /// <summary>
        ///     Curtain system's type name.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        ///     Reference wall.
        /// </summary>
        public Wall ReferenceWall { get; set; }

        /// <summary>
        ///     Reference room.
        /// </summary>
        public SpatialElement Room { get; set; }

        /// <summary>
        ///     Panel type.
        /// </summary>
        public PanelType PanelType { get; set; }

        /// <summary>
        ///     Template file name.
        /// </summary>
        public string TemplateFileName { get; set; }
    }
}