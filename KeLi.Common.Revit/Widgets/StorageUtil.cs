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
     |  |              Creation Time: 01/16/2020 03:33:20 PM |  |  |     |         |      |
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

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace KeLi.Common.Revit.Widgets
{
    /// <summary>
    ///     Storage utility.
    /// </summary>
    public static class StorageUtil
    {
        /// <summary>
        ///     Closes the unactive document that is unsaved.
        ///     You can choose whether to save or not.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="modelPath"></param>
        /// <param name="saveModified"></param>
        public static void CloseUnsavedFile(this Document doc, string modelPath, bool saveModified = true)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (modelPath is null)
                throw new ArgumentNullException(nameof(modelPath));

            if (!string.IsNullOrWhiteSpace(doc.PathName))
                throw new Exception("The document file existed!");

            if (File.Exists(modelPath))
                File.Delete(modelPath);

            if (saveModified)
                doc.SaveAs(modelPath);

            doc.Close(saveModified);
        }

        /// <summary>
        ///     Closes the active or unactive document that is unsaved.
        ///     You can choose whether to save or not.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="uiapp"></param>
        /// <param name="modelPath"></param>
        /// <param name="tmpRvt"></param>
        /// <param name="saveModified"></param>
        public static void CloseUnsavedFile(this Document doc, UIApplication uiapp, string modelPath, string tmpRvt,
            bool saveModified = true)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (modelPath is null)
                throw new ArgumentNullException(nameof(modelPath));

            if (tmpRvt is null)
                throw new ArgumentNullException(nameof(tmpRvt));

            if (!string.IsNullOrWhiteSpace(doc.PathName))
                throw new Exception("The document file existed!");

            if (File.Exists(modelPath))
                File.Delete(modelPath);

            if (saveModified)
                doc.SaveAs(modelPath);

            doc.SafelyClose(uiapp, tmpRvt, saveModified);
        }

        /// <summary>
        ///     Closes the active document path by only covered mode.
        ///     You can choose whether to save or not.
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="tmpRvt"></param>
        /// <param name="saveModified"></param>
        public static void CloseExistedFile(this UIApplication uiapp, string tmpRvt, bool saveModified = true)
        {
            if (uiapp is null)
                throw new ArgumentNullException(nameof(uiapp));

            if (tmpRvt is null)
                throw new ArgumentNullException(nameof(tmpRvt));

            uiapp.ActiveUIDocument.Document.CloseExistedFile(uiapp, tmpRvt, saveModified);
        }

        /// <summary>
        ///     Closes the active document path by only covered mode.
        ///     You can choose whether to save or not.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="uiapp"></param>
        /// <param name="tmpRvt"></param>
        /// <param name="saveModified"></param>
        public static void CloseExistedFile(this Document doc, UIApplication uiapp, string tmpRvt,
            bool saveModified = true)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (uiapp is null)
                throw new ArgumentNullException(nameof(uiapp));

            if (tmpRvt is null)
                throw new ArgumentNullException(nameof(tmpRvt));

            if (string.IsNullOrWhiteSpace(doc.PathName))
                throw new Exception("The document file doesn't exist, please save it!");

            doc.SafelyClose(uiapp, tmpRvt, saveModified);
        }

        /// <summary>
        ///     Closes the document safely.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="uiapp"></param>
        /// <param name="tmpRvt"></param>
        /// <param name="saveModified"></param>
        public static void SafelyClose(this Document doc, UIApplication uiapp, string tmpRvt, bool saveModified = true)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (uiapp is null)
                throw new ArgumentNullException(nameof(uiapp));

            if (tmpRvt is null)
                throw new ArgumentNullException(nameof(tmpRvt));

            if (Equals(uiapp.ActiveUIDocument.Document, doc))
            {
                var modelPath = new FilePath(tmpRvt);
                var opt = new OpenOptions();

                uiapp.OpenAndActivateDocument(modelPath, opt, false);
            }

            // Before closing active document, must open another a document and active it.
            if (saveModified)
            {
                if (string.IsNullOrWhiteSpace(doc.PathName))
                    throw new Exception("The document file doesn't exist, please save it!");

                doc.Close(true);
            }

            else
            {
                doc.Close(false);
            }
        }
    }
}