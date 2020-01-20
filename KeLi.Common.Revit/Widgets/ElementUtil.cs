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
     |  |              Creation Time: 12/23/2019 13:08:20 PM |  |  |     |         |      |
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
using KeLi.Common.Revit.Filters;

namespace KeLi.Common.Revit.Widgets
{
    /// <summary>
    /// Elementy utility.
    /// </summary>
    public static class ElementUtil
    {
        /// <summary>
        /// Gets FamilySymbol list.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<FamilySymbol> GetFamilySymbolList(this Document doc)
        {
            return doc.GetTypeElementList<FamilySymbol>();
        }

        /// <summary>
        /// Gets FamilyInstance list.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<Wall> GetWallList(Document doc)
        {
            return doc.GetInstanceElementList<Wall>();
        }

        /// <summary>
        /// Gets FamilyInstance list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        public static List<FamilyInstance> GetFamilyInstanceList(this Document doc, string symbolName)
        {
            return GetFamilyInstanceList(doc).Where(w => w.Symbol.Name == symbolName).ToList();
        }

        /// <summary>
        /// Gets FamilyInstance list.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<FamilyInstance> GetFamilyInstanceList(this Document doc)
        {
            return doc.GetInstanceElementList<FamilyInstance>();
        }

        /// <summary>
        /// Gets PanelType list.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<SpatialElement> GetSpatialElementList(this Document doc)
        {
            return doc.GetInstanceElementList<SpatialElement>();
        }

        /// <summary>
        /// Gets PanelType list.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<PanelType> GetPanelTypeList(this Document doc)
        {
            return doc.GetTypeElementList<PanelType>().ToList();
        }

        /// <summary>
        /// Gets WallType list.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<WallType> GetWallTypeList(this Document doc)
        {
            return doc.GetTypeElementList<WallType>();
        }

        /// <summary>
        /// Gets the bottom level.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static Level GetBottomLevel(this Document doc)
        {
            return GetLevelList(doc).OrderBy(o => o.Elevation).FirstOrDefault();
        }

        /// <summary>
        /// Gets Level list.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<Level> GetLevelList(this Document doc)
        {
            return doc.GetInstanceElementList<Level>();
        }

        /// <summary>
        /// Gets the element's location point.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static XYZ GetLocationPoint<T>(this T elm) where T: Element
        {
            return elm.Location is LocationPoint pt ? pt.Point : throw new InvalidCastException(elm.Name);
        }

        /// <summary>
        /// Gets the element's location cuve.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static Curve GetLocationCurve<T>(this T elm) where T : Element
        {
            return !(elm.Location is LocationCurve curve) ? throw new InvalidCastException(elm.Name) : curve.Curve;
        }

        /// <summary>
        /// Sets the element's color fill pattern.
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="fillPattern"></param>
        /// <param name="doc"></param>
        /// <param name="color"></param>
        public static void SetColorFill(this Element elm, Element fillPattern, Document doc, Color color)
        {
            var graSetting = doc.ActiveView.GetElementOverrides(elm.Id);

            if (fillPattern != null)
                graSetting.SetProjectionFillPatternId(fillPattern.Id);

            graSetting.SetProjectionFillColor(color);
            doc.ActiveView.SetElementOverrides(elm.Id, graSetting);
        }
    }
}
