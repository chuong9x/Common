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
     |  |              Creation Time: 05/13/2020 06:02:02 PM |  |  |     |         |      |
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

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    ///     Wall sweep info.
    /// </summary>
    public class SweepInfo
    {
        /// <summary>
        ///     Wall sweep info.
        /// </summary>
        /// <param name="profileName"></param>
        /// <param name="materialName"></param>
        /// <param name="distance"></param>
        /// <param name="flip"></param>
        /// <param name="isAbsolute"></param>
        public SweepInfo(string profileName, string materialName, double distance, bool flip, bool isAbsolute = false)
        {
            ProfileName = profileName;
            MaterialName = materialName;
            Distance = distance;
            Flip = flip;
            IsAbsolute = isAbsolute;
        }

        /// <summary>
        ///     ProfileName
        /// </summary>
        public string ProfileName { get; set; }

        /// <summary>
        ///     MaterialName
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        ///     Distance
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        ///     Flip
        /// </summary>
        public bool Flip { get; set; }

        /// <summary>
        ///     IsAbsolute
        /// </summary>
        public bool IsAbsolute { get; set; }
    }
}