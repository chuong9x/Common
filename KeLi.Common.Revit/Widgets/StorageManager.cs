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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using KeLi.Common.Revit.Properties;
using KeLi.Common.Tool.Dir;
using KeLi.Common.Tool.Other;

namespace KeLi.Common.Revit.Widgets
{
    /// <summary>
    /// 
    /// </summary>
    public static class StorageManager
    {
        /// <summary>
        ///     Gets the last file path.
        /// </summary>
        /// <param name="subDir"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static string GetLastFile(string subDir, string searchPattern)
        {
            if (subDir is null)
                throw new ArgumentNullException(nameof(subDir));

            if (searchPattern is null)
                throw new ArgumentNullException(nameof(searchPattern));

            var subDirPath = CreateSubDirPath(subDir);

            return Directory.GetFiles(subDirPath,  searchPattern).Max();
        }

        /// <summary>
        ///     Gets a new log name.
        /// </summary>
        /// <param name="subDir"></param>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public static string GetNewLogFile(string subDir, string moduleName)
        {
            if (subDir is null)
                throw new ArgumentNullException(nameof(subDir));

            if (moduleName is null)
                throw new ArgumentNullException(nameof(moduleName));

            var subDirPath = CreateSubDirPath(subDir);

            return Path.Combine(subDirPath, moduleName + "-" + DateTime.Now.ToString("MMddHHmm") + ".log");
        }

        /// <summary>
        ///     Formats the exception to StringBuilder object.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="ex"></param>
        public static void FormatExceptionMsg(this StringBuilder sb, Exception ex)
        {
            sb.AppendLine(ex.Message);

            var stackTrack = ex.StackTrace.Replace(Resources.At, Environment.NewLine + Resources.At);

            stackTrack = stackTrack.Replace(Resources.In, Environment.NewLine + Resources.In);

            stackTrack = stackTrack.Replace("at ", "\r\nat ");

            stackTrack = stackTrack.Replace("in ", "\r\nin ");

            sb.AppendLine(stackTrack);
        }

        /// <summary>
        ///     Writes log, if showDlg is true, can show dialog.
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteLog(this Exception ex)
        {
            var sb = new StringBuilder();

            sb.FormatExceptionMsg(ex);

            sb.WriteLog("Error");
        }

        /// <summary>
        ///     Writes log, if showDlg is true, can show dialog.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="moduleName"></param>
        /// <param name="minLineNum"></param>
        public static void WriteLog(this StringBuilder sb, string moduleName, int minLineNum = 2)
        {
            if (sb is null)
                throw new ArgumentNullException(nameof(sb));

            if (moduleName is null)
                throw new ArgumentNullException(nameof(moduleName));

            var showDlg = Convert.ToBoolean(ConfigUtil.GetValue("IsShowLog"));

            WriteLog(sb, moduleName, showDlg, minLineNum);
        }

        /// <summary>
        ///     Writes log, if showDlg is true, can show dialog.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="moduleName"></param>
        /// <param name="showDlg"></param>
        /// <param name="minLineNum"></param>
        public static void WriteLog(this StringBuilder sb, string moduleName, bool showDlg, int minLineNum = 1)
        {
            if (sb is null)
                throw new ArgumentNullException(nameof(sb));

            if (GetSubStringCount(sb.ToString(), Environment.NewLine) < minLineNum)
                return;

            if (moduleName == null)
                moduleName = DateTime.Now.ToString("yyyyMMddHHmmssffff");

            var result = GetNewLogFile(string.Empty, moduleName);

            File.WriteAllText(result, sb.ToString());

            if (showDlg)
                Process.Start(result);
        }

        /// <summary>
        ///     Writes log, if showDlg is true, can show dialog.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="showDlg"></param>
        public static void WriteLog(this StringBuilder sb, bool showDlg = true)
        {
            if (sb is null)
                throw new ArgumentNullException(nameof(sb));

            var result = GetNewLogFile(string.Empty,DateTime.Now.ToString("yyyyMMddHHmmssffff"));

            File.WriteAllText(result, sb.ToString());

            if (showDlg)
                Process.Start(result);
        }

        /// <summary>
        ///     Creates a new path with sub dir node.
        /// </summary>
        /// <param name="fullStr"></param>
        /// <param name="subStr"></param>
        /// <returns></returns>
        public static int GetSubStringCount(string fullStr, string subStr)
        {
            if (!fullStr.Contains(subStr))
                return 0;

            var replacedStr = fullStr.Replace(subStr, string.Empty);

            return (fullStr.Length - replacedStr.Length) / subStr.Length;
        }

        /// <summary>
        ///     Creates a new path with sub dir node.
        /// </summary>
        /// <param name="pathNode"></param>
        /// <returns></returns>
        public static string CreateSubDirPath(string pathNode)
        {
            if (pathNode is null)
                throw new ArgumentNullException(nameof(pathNode));

            var baseDirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return DirectoryPlus.CombinePlus(baseDirPath, pathNode);
        }
    }
}