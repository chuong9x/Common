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
     |  |              Creation Time: 03/17/2020 05:50:20 PM |  |  |     |         |      |
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

namespace KeLi.Common.Tool.Dir
{
    /// <summary>
    ///     Directory plus.
    /// </summary>
    public class DirectoryPlus
    {
        /// <summary>
        ///     Combines some paths.
        ///     If a folder not exists, it will create.
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string CombinePlus(params string[] paths)
        {
            if (paths is null)
                throw new ArgumentNullException(nameof(paths));

            if (!paths[0].Contains(":"))
                throw new InvalidDataException($"The path's root '{paths[0]}' is invalid!");

            var basePath = CreateFullPath(paths[0]);

            if (paths.Length <= 1)
                return basePath;

            var path = basePath;

            for (var i = 1; i < paths.Length; i++)
            {
                var tmpPath = paths[i].Replace(@"\\", @"\").Trim('\\');

                path = Path.Combine(path, tmpPath);

                path = CreateFullPath(path);
            }

            return path;
        }

        /// <summary>
        ///     Creates the full directory path.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        private static string CreateFullPath(string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath))
                throw new ArgumentNullException(nameof(fullPath));

            fullPath = fullPath.Replace(@"\\", @"\").Trim('\\');

            if (File.Exists(fullPath))
                return fullPath;

            var nodes = fullPath.Split('\\');

            if (!nodes[0].Contains(":"))
                throw new InvalidDataException($"The path's root '{nodes[0]}' is invalid!");

            var path = nodes[0] + @"\";

            if (nodes.Length <= 1)
                return path;

            for (var i = 1; i < nodes.Length; i++)
            {
                path = Path.Combine(path, nodes[i]);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }

            return path;
        }
    }
}