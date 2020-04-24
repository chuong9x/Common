using System;
/*
 * MIT License
 *
 * Copyright(c) 2020 KeLi
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
     |  |              Creation Time: 04/09/2020 07:08:41 PM |  |  |     |         |      |
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

using System.IO;
using System.Linq;

using Autodesk.Revit.DB;

using KeLi.Common.Revit.Filters;
using KeLi.Common.Revit.Properties;

namespace KeLi.Common.Revit.Widgets
{
    /// <summary>
    /// 	Drawing utility.
    /// </summary>
    public static class DrawingUtil
    {
        /// <summary>
        /// 	Exports drawing list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="drawingPath"></param>
        /// <param name="setupName"></param>
        public static void ExportDrawingList(this Document doc, string drawingPath, string setupName = null)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            var viewSheets = doc.GetInstanceList<ViewSheet>();

            var drawings = viewSheets.Where(w => w.ViewType == ViewType.DrawingSheet);

            var viewIds = drawings.Select(s => s.Id).ToList();

            if (viewIds.Count == 0)
                return;

            var setupNames = BaseExportOptions.GetPredefinedSetupNames(doc);

            if (string.IsNullOrWhiteSpace(setupName))
                setupName = setupNames.FirstOrDefault();

            var dwgOpts = DWGExportOptions.GetPredefinedOptions(doc, setupName);

            dwgOpts.MergedViews = true;

            var docName = Path.GetFileNameWithoutExtension(doc.PathName);

            doc.Export(drawingPath, string.Empty, viewIds, dwgOpts);

            var filePaths = Directory.GetFiles(drawingPath, "*.*");

            foreach (var filePath in filePaths)
            {
                var newFilePath = filePath.Replace($"{docName}-{Resources.Draw_CHS} - ", string.Empty);

                if (File.Exists(filePath))
                    File.Move(filePath, newFilePath);
            }
        }
    }
}
