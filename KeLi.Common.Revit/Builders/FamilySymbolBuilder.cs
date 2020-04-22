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
     |  |              Creation Time: 01/15/2020 07:39:20 PM |  |  |     |         |      |
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
using System.IO;
using System.Linq;

using Autodesk.Revit.DB;

using KeLi.Common.Revit.Converters;
using KeLi.Common.Revit.Widgets;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    ///     FamilySymbol builder.
    /// </summary>
    public static class FamilySymbolBuilder
    {
        /// <summary>
        ///     Creates a new extrusion symbol.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="parm"></param>
        /// <returns></returns>
        public static Document CreateExtrusion(this Document doc, ExtrudeParameter parm)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (parm is null)
                throw new ArgumentNullException(nameof(parm));
            
            var tplPath = doc.GetTemplateFilePath();

            if (!File.Exists(tplPath))
                throw new FileNotFoundException(tplPath);

            var fdoc = doc.Application.NewFamilyDocument(tplPath);

            var profile = ResetCurveArrArray(parm.Boundary);

            if (profile is null)
                throw new NullReferenceException(nameof(profile));

            fdoc.AutoTransaction(() =>
            {
                var skectchPlane = SketchPlane.Create(fdoc, parm.Plane);

                if (skectchPlane is null)
                    throw new NullReferenceException(nameof(skectchPlane));

                fdoc.FamilyCreate.NewExtrusion(true, profile, skectchPlane, parm.Thick);
            });

            return fdoc;
        }

        /// <summary>
        ///     Creates a new sweep symbol with transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="parm"></param>
        /// <returns></returns>
        public static Document CreateSweep(this Document doc, SweepParameter parm)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            var tplPath = doc.GetTemplateFilePath();

            if (!File.Exists(tplPath))
                throw new FileNotFoundException(tplPath);

            var fdoc = doc.Application.NewFamilyDocument(tplPath);

            var curveLoops = ResetCurveArrArray(parm.Profile);

            if (curveLoops is null)
                throw new NullReferenceException(nameof(curveLoops));

            fdoc.AutoTransaction(() =>
            {
                var profile = doc.Application.Create.NewCurveLoopsProfile(curveLoops);

                if (profile is null)
                    throw new NullReferenceException(nameof(profile));

                fdoc.FamilyCreate.NewSweep(true, parm.Path, profile, parm.Index, parm.Location);
            });

            return fdoc;
        }

        /// <summary>
        ///     Loads family.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="rfaPath"></param>
        /// <returns></returns>
        public static FamilySymbol LoadFamily(this Document doc, string rfaPath)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (rfaPath is null)
                throw new ArgumentNullException(nameof(rfaPath));

            if(!File.Exists(rfaPath))
                throw new FileNotFoundException(rfaPath);

            doc.LoadFamily(rfaPath, out var family);

            var symbolId = family.GetFamilySymbolIds().FirstOrDefault();

            var result = doc.GetElement(symbolId) as FamilySymbol;

            if (result != null && !result.IsActive)
                result.Activate();

            return result;
        }

        /// <summary>
        ///     Loads family.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="fdoc"></param>
        /// <returns></returns>
        public static FamilySymbol NewLoadFamily(this Document doc, Document fdoc)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (fdoc is null)
                throw new ArgumentNullException(nameof(fdoc));

            var family = fdoc.LoadFamily(doc);

            var symbolId = family.GetFamilySymbolIds().FirstOrDefault();

            var result = doc.GetElement(symbolId) as FamilySymbol;

            if (result != null && !result.IsActive)
                result.Activate();

            return result;
        }

        /// <summary>
        ///     Resets the family symbol profile's location to zero point.
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public static CurveArrArray ResetCurveArrArray(CurveArrArray profile)
        {
            if (profile is null)
                throw new ArgumentNullException(nameof(profile));

            var results = new CurveArrArray();

            var pts = profile.ToCurveList().Select(s => s.GetEndPoint(0));

            pts = pts.OrderBy(o => o.Z).ThenBy(o => o.Y).ThenBy(o => o.X);

            var location = pts.FirstOrDefault();

            foreach (CurveArray lines in profile)
            {
                var tmpLines = new CurveArray();

                foreach (var line in lines.Cast<Line>())
                {
                    if (line.Length < 1e-2)
                        throw new InvalidDataException(line.ToString());

                    var pt1 = line.GetEndPoint(0) - location;

                    var pt2 = line.GetEndPoint(1) - location;

                    var newLine = Line.CreateBound(pt1, pt2);

                    tmpLines.Append(newLine);
                }

                results.Append(tmpLines);
            }

            return results;
        }
    }
}