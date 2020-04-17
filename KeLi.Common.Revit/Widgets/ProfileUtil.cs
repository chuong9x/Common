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
     |  |              Creation Time: 04/16/2020 01:57:20 PM |  |  |     |         |      |
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
using System.Linq;

using Autodesk.Revit.DB;

using KeLi.Common.Revit.Converters;
using KeLi.Common.Revit.Filters;

namespace KeLi.Common.Revit.Widgets
{
    /// <summary>
    ///     Profile utility.
    /// </summary>
    public static class ProfileUtil
    {
        /// <summary>
        ///     Gets sweep's profile.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="familyPath"></param>
        /// <returns></returns>
        public static CurveArrArray GetSweepProfile(Document doc, string familyPath)
        {
            if (doc == null)
                throw new NullReferenceException(nameof(doc));

            if (familyPath == null)
                throw new NullReferenceException(nameof(familyPath));

            var profileDoc = doc.Application.OpenDocumentFile(familyPath);

            var detailCurves = profileDoc.GetInstanceElementList<CurveElement>();

            var curves = detailCurves.Select(s => s.GeometryCurve);

            return curves.ToCurveArrArray();
        }

        /// <summary>
        ///     Gets sweep's profile.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        public static CurveArrArray GetSweepProfile2(Document doc, string symbolName)
        {
            if (doc == null)
                throw new NullReferenceException(nameof(doc));

            if (symbolName == null)
                throw new NullReferenceException(nameof(symbolName));

            var symbol = doc.GetTypeElementList<FamilySymbol>().FirstOrDefault(f => f.Name == symbolName);

            if(symbol == null)
                throw new NullReferenceException(nameof(symbol));

            var profileDoc = doc.EditFamily(symbol.Family);

            var detailCurves = profileDoc.GetInstanceElementList<CurveElement>();

            var curves = detailCurves.Select(s => s.GeometryCurve);

            return curves.ToCurveArrArray();
        }


    }
}