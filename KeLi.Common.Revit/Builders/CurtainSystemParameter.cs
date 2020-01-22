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
        ///     Curtain system parameter.
        /// </summary>
        /// <param name="refWall"></param>
        /// <param name="refCenter"></param>
        /// <param name="pnlType"></param>
        /// <param name="tplFileName"></param>
        public CurtainSystemParameter(Wall refWall, XYZ refCenter, PanelType pnlType, string tplFileName)
        {
            RefWall = refWall ?? throw new ArgumentNullException(nameof(refWall));
            RefCenter = refCenter ?? throw new ArgumentNullException(nameof(refCenter));
            PanelType = pnlType ?? throw new ArgumentNullException(nameof(pnlType));
            TemplateFileName = tplFileName ?? throw new ArgumentNullException(nameof(tplFileName));
        }

        /// <summary>
        ///     Reference wall.
        /// </summary>
        public Wall RefWall { get; set; }

        /// <summary>
        ///     Reference point, such as, room center.
        /// </summary>
        public XYZ RefCenter { get; set; }

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