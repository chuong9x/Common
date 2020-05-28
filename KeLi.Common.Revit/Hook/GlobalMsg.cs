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
     |  |              Creation Time: 05/29/2020 07:08:41 PM |  |  |     |         |      |
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

namespace KeLi.Common.Revit.Hook
{
    /// <summary>
    /// 
    /// </summary>
    public static class GlobalMsg
    {
        /// <summary>
        /// 
        /// </summary>
        public const int WH_MOUSE_LL = 14;

        /// <summary>
        /// 
        /// </summary>
        public const int WH_KEYBOARD_LL = 13;

        /// <summary>
        /// 
        /// </summary>
        public const int WM_KEYDOWN = 0x100;

        /// <summary>
        /// 
        /// </summary>
        public const int WM_KEYUP = 0x101;

        /// <summary>
        /// 
        /// </summary>
        public const int WM_SYSKEYDOWN = 0x104;

        /// <summary>
        /// 
        /// </summary>
        public const int WM_SYSKEYUP = 0x105;

        /// <summary>
        /// 
        /// </summary>
        public const int WM_MOUSEMOVE = 0x200;

        /// <summary>
        /// 
        /// </summary>
        public const int WM_LBUTTONDOWN = 0x201;

        /// <summary>
        /// 
        /// </summary>
        public const int WM_RBUTTONDOWN = 0x204;

        /// <summary>
        /// 
        /// </summary>
        public const int WM_MBUTTONDOWN = 0x207;

        /// <summary>
        /// 
        /// </summary>
        public const int WM_LBUTTONUP = 0x202;

        /// <summary>
        /// 
        /// </summary>
        public const int WM_RBUTTONUP = 0x205;

        /// <summary>
        /// 
        /// </summary>
        public const int WM_MBUTTONUP = 0x208;

        /// <summary>
        /// 
        /// </summary>
        public const int WM_LBUTTONDBLCLK = 0x203;

        /// <summary>
        /// 
        /// </summary>
        public const int WM_RBUTTONDBLCLK = 0x206;

        /// <summary>
        /// 
        /// </summary>
        public const int WM_MBUTTONDBLCLK = 0x209;
    }
}