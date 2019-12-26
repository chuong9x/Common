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
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    /// Family instance builder.
    /// </summary>
    public static class FamilyInstanceBuilder
    {
        /// <summary>
        /// Creates a new family instance.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="location"></param>
        /// <param name="symbol"></param>
        /// <param name="lvl"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static FamilyInstance CreateFamilyInstance(this Document doc, XYZ location, FamilySymbol symbol, Level lvl,
            StructuralType type)
        {
            return doc.Create.NewFamilyInstance(location, symbol, lvl, type);
        }

        /// <summary>
        /// Creates a new family instance.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="location"></param>
        /// <param name="symbol"></param>
        /// <param name="elm"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static FamilyInstance CreateFamilyInstance(this Document doc, XYZ location, FamilySymbol symbol, Element elm,
            StructuralType type)
        {
            return doc.Create.NewFamilyInstance(location, symbol, elm, type);
        }

        /// <summary>
        /// Creates a new family instance.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="location"></param>
        /// <param name="symbol"></param>
        /// <param name="elm"></param>
        /// <param name="lvl"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static FamilyInstance CreateFamilyInstance(this Document doc, XYZ location, FamilySymbol symbol, Element elm,
            Level lvl, StructuralType type)
        {
            return doc.Create.NewFamilyInstance(location, symbol, elm, lvl, type);
        }

        /// <summary>
        /// Creates a new family instance.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="location"></param>
        /// <param name="symbol"></param>
        /// <param name="elm"></param>
        /// <param name="type"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static FamilyInstance CreateFamilyInstance(this Document doc, XYZ location, FamilySymbol symbol,
            XYZ direction, Element elm, StructuralType type)
        {
            return doc.Create.NewFamilyInstance(location, symbol, direction, elm, type);
        }

        /// <summary>
        /// Creates a new instance of an adaptive component family.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="symbol"></param>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static FamilyInstance CreateFamilyInstance(this Document doc, FamilySymbol symbol, List<XYZ> pts)
        {
            // Creates a new instance of an adaptive component family.
            var result = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(doc, symbol);

            // Gets the placement points of this instance.
            var placePointIds = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(result);

            for (var i = 0; i < placePointIds.Count; i++)
            {
                if (doc.GetElement(placePointIds[i]) is ReferencePoint point)
                    point.Position = pts[i];
            }

            return result;
        }

        /// <summary>
        /// Creates a new family symbol.
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="rfa"></param>
        /// <param name="profile"></param>
        /// <param name="plane"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static FamilySymbol CreateFamilySymbol(this UIApplication uiapp, string rfa, CurveArrArray profile,
            SketchPlane plane, double end)
        {
            var doc = uiapp.ActiveUIDocument.Document;
            var fdoc = uiapp.Application.NewFamilyDocument(rfa);

            fdoc.CreateExtrusion(profile, plane, end);

            return doc.GetElement(fdoc.LoadFamily(doc).GetFamilySymbolIds().FirstOrDefault()) as FamilySymbol;
        }

        /// <summary>
        /// Creates a new family symbol.
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="rfa"></param>
        /// <param name="profile"></param>
        /// <param name="path"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static FamilySymbol CreateFamilySymbol(this UIApplication uiapp, string rfa, SweepProfile profile,
            ReferenceArray path, int index)
        {
            var doc = uiapp.ActiveUIDocument.Document;
            var fdoc = uiapp.Application.NewFamilyDocument(rfa);

            fdoc.CreateSweep(profile, path, index);

            return doc.GetElement(fdoc.LoadFamily(doc).GetFamilySymbolIds().FirstOrDefault()) as FamilySymbol;
        }

        /// <summary>
        /// Creates a new family symbol.
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="rfa"></param>
        /// <returns></returns>
        public static FamilySymbol CreateFamilySymbol(this UIApplication uiapp, string rfa)
        {
            var doc = uiapp.ActiveUIDocument.Document;

            doc.LoadFamily(rfa, out var family);

            return doc.GetElement(family.GetFamilySymbolIds().FirstOrDefault()) as FamilySymbol;
        }

        /// <summary>
        /// Creates a new family symbol.
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="rft"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static FamilySymbol CreateFamilySymbol(this UIApplication uiapp, string rft, Action<Document> act)
        {
            var doc = uiapp.ActiveUIDocument.Document;
            var fdoc = uiapp.Application.NewFamilyDocument(rft);

            act.Invoke(fdoc);

            return doc.GetElement(fdoc.LoadFamily(doc).GetFamilySymbolIds().FirstOrDefault()) as FamilySymbol;
        }
    }
}