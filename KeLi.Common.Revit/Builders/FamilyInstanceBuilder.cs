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
        /// <param name="parm"></param>
        /// <returns></returns>
        public static FamilyInstance CreateFamilyInstance(this Document doc, FamilyInstanceParameter parm)
        {
            if (parm == null)
                throw new ArgumentNullException(nameof(parm));

            if (doc.IsFamilyDocument)
                return doc.FamilyCreate.NewFamilyInstance(parm.Location, parm.Symbol, parm.Level, parm.Type);

            return doc.Create.NewFamilyInstance(parm.Location, parm.Symbol, parm.Level, parm.Type);
        }

        /// <summary>
        /// Creates a new family instance with NonStructural type.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="location"></param>
        /// <param name="symbol"></param>
        /// <param name="lvl"></param>
        /// <returns></returns>
        public static FamilyInstance CreateNonStructuralInstance(this Document doc, XYZ location, FamilySymbol symbol, Level lvl)
        {
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));

            if (location == null)
                throw new ArgumentNullException(nameof(location));

            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));

            if (lvl == null)
                throw new ArgumentNullException(nameof(lvl));

            var parm = new FamilyInstanceParameter(location, symbol, lvl, StructuralType.NonStructural);

            return doc.CreateFamilyInstance(parm);
        }

        /// <summary>
        /// Creates a new instance of an adaptive component family.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="symbol"></param>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static FamilyInstance CreateFamilyInstance(this Document doc, FamilySymbol symbol, IEnumerable<XYZ> pts)
        {
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));

            if (pts == null)
                throw new ArgumentNullException(nameof(pts));

            var tmpPts = pts.ToList();

            // Creates a new instance of an adaptive component family.
            var result = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(doc, symbol);

            // Gets the placement points of this instance.
            var placePointIds = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(result);

            for (var i = 0; i < placePointIds.Count; i++)
            {
                if (doc.GetElement(placePointIds[i]) is ReferencePoint point)
                    point.Position = tmpPts[i];
            }

            return result;
        }
    }
}