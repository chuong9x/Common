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
     |  |              Creation Time: 04/15/2020 19:20:20 PM |  |  |     |         |      |
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

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;

using KeLi.Common.Revit.Properties;

namespace KeLi.Common.Revit.Widgets
{
    /// <summary>
    /// Template path unitility.
    /// </summary>
    public static class TemplatePathUtil
    {
        /// <summary>
        ///     Gets the general template file path.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static string GeTemplateFilePath(this Document doc)
        {
            if (doc == null)
                throw new NullReferenceException(nameof(doc));

            var tplName = doc.Application.Language.GetGeneralTplName();

            return doc.GetTemplateFilePath(tplName);
        }

        /// <summary>
        ///     Gets the template file path.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetTemplateFilePath(this Document doc, string fileName)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (fileName is null)
                throw new ArgumentNullException(nameof(fileName));

            var app = doc.Application;

            return Path.Combine(app.FamilyTemplatePath.Replace("English_I", "English"), fileName);
        }

        /// <summary>
        ///     Gets the general template file name.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetGeneralTplName(this LanguageType type)
        {
            switch (type)
            {
                case LanguageType.Chinese_Simplified:
                    return Resources.GeneralTemplateName_CHS;

                case LanguageType.English_USA:
                    return "Metric Generic Model.rft";

                #if !R2016 && !R2017
                case LanguageType.English_GB:
                    return "Metric Generic Model.rft";
                #endif

                default:
                    throw new NotSupportedException($"No support {type} language!");
            }
        }
    }
}