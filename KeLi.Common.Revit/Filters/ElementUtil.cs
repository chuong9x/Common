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

using static Autodesk.Revit.DB.SpatialElementBoundaryLocation;

namespace KeLi.Common.Revit.Filters
{
    /// <summary>
    ///     Elementy utility.
    /// </summary>
    public static class ElementUtil
    {
        /// <summary>
        ///     Square foot to spaure meter.
        /// </summary>
        private const double FT2_TO_M2 = 0.092903;

        /// <summary>
        ///     Gets FamilySymbol list.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<FamilySymbol> GetFamilySymbolList(this Document doc)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            return doc.GetTypeElementList<FamilySymbol>();
        }

        /// <summary>
        ///     Gets FamilyInstance list.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<Wall> GetWallList(this Document doc)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            return doc.GetInstanceElementList<Wall>();
        }

        /// <summary>
        ///     Gets FamilyInstance list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        public static List<FamilyInstance> GetFamilyInstanceList(this Document doc, string symbolName)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (symbolName is null)
                throw new ArgumentNullException(nameof(symbolName));

            return GetFamilyInstanceList(doc).Where(w => w.Symbol.Name == symbolName).ToList();
        }

        /// <summary>
        ///     Gets FamilyInstance list.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<FamilyInstance> GetFamilyInstanceList(this Document doc)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            return doc.GetInstanceElementList<FamilyInstance>();
        }

        /// <summary>
        ///     Gets room list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="isValid"></param>
        /// <returns></returns>
        public static List<SpatialElement> GetSpatialElementList(this Document doc, bool isValid = true)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            var results = doc.GetInstanceElementList<SpatialElement>();

            if (isValid)
                results = results.Where(w => w?.Location != null && w.Area > 1e-6).ToList();

            return results;
        }

        /// <summary>
        ///     Gets PanelType list.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<PanelType> GetPanelTypeList(this Document doc)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            return doc.GetTypeElementList<PanelType>().ToList();
        }

        /// <summary>
        ///     Gets WallType list.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<WallType> GetWallTypeList(this Document doc)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            return doc.GetTypeElementList<WallType>();
        }

        /// <summary>
        ///     Gets the bottom level.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static Level GetBottomLevel(this Document doc)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            return GetLevelList(doc).OrderBy(o => o.Elevation).FirstOrDefault();
        }

        /// <summary>
        ///     Gets Level list.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<Level> GetLevelList(this Document doc)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            return doc.GetInstanceElementList<Level>();
        }

        /// <summary>
        ///     Gets the element's location point.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static XYZ GetLocationPoint<T>(this T elm) where T : Element
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            return elm.Location is LocationPoint pt ? pt.Point : throw new InvalidCastException(elm.Name);
        }

        /// <summary>
        ///     Gets the element's location cuve.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static Curve GetLocationCurve<T>(this T elm) where T : Element
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            return !(elm.Location is LocationCurve curve) ? throw new InvalidCastException(elm.Name) : curve.Curve;
        }

        /// <summary>
        ///     Gets intersect element list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="room"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<T> GetIntersectElements<T>(this SpatialElement room, Document doc) where T : Element
        {
            if (room is null)
                throw new ArgumentNullException(nameof(room));

            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            return room.GetIntersectElements(doc).Where(w => w is T).Cast<T>().ToList();
        }

        /// <summary>
        ///     Gets intersect element list.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<Element> GetIntersectElements(this SpatialElement room, Document doc)
        {
            if (room is null)
                throw new ArgumentNullException(nameof(room));

            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            var opt = new SpatialElementBoundaryOptions  { SpatialElementBoundaryLocation = Center };

            var calc = new SpatialElementGeometryCalculator(doc, opt);

            var solid = calc.CalculateSpatialElementGeometry(room).GetGeometry();

            var instFilter = new FilteredElementCollector(doc).WhereElementIsNotElementType();

            var itstFilter = new ElementIntersectsSolidFilter(solid);

            return instFilter.WherePasses(itstFilter).ToList();
        }

        /// <summary>
        ///     Sets the element's color fill pattern.
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="fillPattern"></param>
        /// <param name="doc"></param>
        /// <param name="color"></param>
        public static void SetColorFill(this Element elm, Element fillPattern, Document doc, Color color)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (fillPattern is null)
                throw new ArgumentNullException(nameof(fillPattern));

            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (color is null)
                throw new ArgumentNullException(nameof(color));

            var graSetting = doc.ActiveView.GetElementOverrides(elm.Id);

            graSetting.SetProjectionFillPatternId(fillPattern.Id);

            graSetting.SetProjectionFillColor(color);

            doc.ActiveView.SetElementOverrides(elm.Id, graSetting);
        }

        /// <summary>
        ///     Gets the element's projection area.
        /// </summary>
        /// <param name="elm">A element</param>
        /// <remarks>Returns projection area, that area unit is square meter.</remarks>
        /// <exception cref="T:Autodesk.Revit.Exceptions.ArgumentNullException">The input element is invalid.</exception>
        /// <returns>Returns projection area.</returns>
        public static double GetShadowArea(this Element elm)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            var areas = new List<double>();
            var geo = elm.get_Geometry(new Options());

            foreach (var instance in geo.Select(s => s as GeometryInstance))
            {
                if (instance is null)
                    continue;

                foreach (var item in instance.GetInstanceGeometry())
                {
                    var solid = item as Solid;

                    if (null == solid || solid.Faces.Size <= 0)
                        continue;

                    var plane = Plane.CreateByOriginAndBasis(XYZ.Zero, XYZ.BasisX, XYZ.BasisY);

                    ExtrusionAnalyzer analyzer;

                    try
                    {
                        analyzer = ExtrusionAnalyzer.Create(solid, plane, XYZ.BasisZ);
                    }
                    catch
                    {
                        continue;
                    }

                    if (analyzer is null)
                        continue;

                    areas.Add(analyzer.GetExtrusionBase().Area * FT2_TO_M2);
                }
            }

            return areas.Max();
        }
    }
}