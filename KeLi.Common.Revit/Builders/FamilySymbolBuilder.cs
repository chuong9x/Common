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
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using KeLi.Common.Revit.Geometry;
using KeLi.Common.Revit.Widgets;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    /// FamilySymbol builder.
    /// </summary>
    public static class FamilySymbolBuilder
    {
        /// <summary>
        /// Creates a new extrusion symbol.
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="symbolParm"></param>
        /// <param name="familyParm"></param>
        /// <returns></returns>
        public static FamilySymbol CreateExtrusionSymbol(this UIApplication uiapp, FamilySymbolParm symbolParm, FamilyParm familyParm = null)
        {
            var doc = uiapp.ActiveUIDocument.Document;
            var templateFilePath = uiapp.GeTemplateFilePath(symbolParm.TemplateFileName);
            var fdoc = uiapp.Application.NewFamilyDocument(templateFilePath);

            fdoc.AutoTransaction(() =>
            {
                var skectchPlane = fdoc.CreateSketchPlane(symbolParm.Plane);
                var extrusion = fdoc.FamilyCreate.NewExtrusion(true, symbolParm.ExtrusionProfile, skectchPlane, symbolParm.End);

                ElementTransformUtils.MoveElement(fdoc, extrusion.Id, -extrusion.GetBoundingBox(fdoc).Min);
            });

            if (familyParm != null)
                fdoc.SaveAsAndClose(uiapp, familyParm.RfaPath, familyParm.TmpPath);

            return doc.GetFamilySymbol(fdoc);
        }

        /// <summary>
        /// Creates a new sweep symbol.
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="symbolParm"></param>
        /// <param name="familyParm"></param>
        /// <returns></returns>
        public static FamilySymbol CreateSweepSymbol(this UIApplication uiapp, FamilySymbolParm symbolParm, FamilyParm familyParm)
        {
            var doc = uiapp.ActiveUIDocument.Document;
            var templateFilePath = uiapp.GeTemplateFilePath(symbolParm.TemplateFileName);
            var fdoc = uiapp.Application.NewFamilyDocument(templateFilePath);

            fdoc.AutoTransaction(() =>
            {
                var sweep = fdoc.FamilyCreate.NewSweep(true, symbolParm.SweepPath, symbolParm.SweepProfile, symbolParm.Index, ProfilePlaneLocation.Start);

                ElementTransformUtils.MoveElement(fdoc, sweep.Id, -sweep.GetBoundingBox(fdoc).Min);
            });

            fdoc.SaveAsAndClose(uiapp, familyParm.RfaPath, familyParm.TmpPath);

            return doc.GetFamilySymbol(fdoc);
        }

        /// <summary>
        /// Creates a new family symbol.
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="rfaPath"></param>
        /// <returns></returns>
        public static FamilySymbol CreateFamilySymbol(this UIApplication uiapp, string rfaPath)
        {
            var doc = uiapp.ActiveUIDocument.Document;

            doc.LoadFamily(rfaPath, out var family);

            return doc.GetFamilySymbol(family);
        }

        /// <summary>
        /// Creates a new family symbol.
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="templateFilePath"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static FamilySymbol CreateFamilySymbol(this UIApplication uiapp, string templateFilePath, Action<Document> act)
        {
            var doc = uiapp.ActiveUIDocument.Document;
            var fdoc = uiapp.Application.NewFamilyDocument(templateFilePath);

            fdoc.AutoTransaction(() => act.Invoke(fdoc));

            return doc.GetFamilySymbol(fdoc);
        }

        /// <summary>
        /// Gets the first family symbol from family document.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="fdoc"></param>
        /// <returns></returns>
        public static FamilySymbol GetFamilySymbol(this Document doc, Document fdoc)
        {
            var family = fdoc.LoadFamily(doc);

            return doc.GetFamilySymbol(family);
        }

        /// <summary>
        /// Gets the first family symbol from family document.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="family"></param>
        /// <returns></returns>
        public static FamilySymbol GetFamilySymbol(this Document doc, Family family)
        {
            var symbolId = family.GetFamilySymbolIds().FirstOrDefault();
            var result = doc.GetElement(symbolId) as FamilySymbol;

            doc.AutoTransaction(() =>
            {
                if (result != null && !result.IsActive)
                    result.Activate();
            });

            return result;
        }

        /// <summary>
        /// Gets the template file path.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GeTemplateFilePath(this Application app, string fileName)
        {
            return Path.Combine(app.FamilyTemplatePath, fileName);
        }

        /// <summary>
        /// Gets the template file path.
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GeTemplateFilePath(this UIApplication uiapp, string fileName)
        {
            return uiapp.Application.GeTemplateFilePath(fileName);
        }
    }
}