/*
 * MIT License
 *
 * Copyright(c) 2019 kelicto
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
     |  |              Author: kelicto                       |  |  |     |         |      |
     |  |              Email: kelistudy@163.com              |  |  |     |         |      |
     |  |              Creation Time: 10/30/2019 07:08:41 PM |  |  |     |         |      |
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
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.UI;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

namespace KeLi.Common.Revit.Widget
{
    /// <summary>
    /// Custom ribbon utility.
    /// </summary>
    public static class RibbonUtil
    {
        /// <summary>
        /// Adds a button.
        /// </summary>
        /// <param name="pnl"></param>
        /// <param name="pbd"></param>
        /// <returns></returns>
        public static void AddButton(this RibbonPanel pnl, PushButtonData pbd)
        {
            if (pnl == null)
                throw new ArgumentNullException(nameof(pnl));

            if (!(pnl.AddItem(pbd) is PushButton btn))
                return;

            btn.ToolTip = pbd.ToolTip;
            btn.LongDescription = pbd.LongDescription;
            btn.LargeImage = pbd.LargeImage;
        }

        /// <summary>
        /// Adds a button.
        /// </summary>
        /// <param name="pnl"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static void AddButton<T>(this RibbonPanel pnl, ButtonInfo<T> info) where T : IExternalCommand
        {
            if (pnl == null)
                throw new ArgumentNullException(nameof(pnl));

            if (info == null)
                throw new ArgumentNullException(nameof(info));

            if (!(pnl.AddItem(info.CreatePbd()) is PushButton btn))
                return;

            btn.ToolTip = info.ToolTip;
            btn.LongDescription = info.LongDescription;
            btn.LargeImage = info.LargeImage;
        }

        /// <summary>
        /// Adds a button set.
        /// </summary>
        /// <param name="pnl"></param>
        /// <param name="pbds"></param>
        /// <returns></returns>
        public static void AddButtons(this RibbonPanel pnl, List<PushButtonData> pbds)
        {
            if (pnl == null)
                throw new ArgumentNullException(nameof(pnl));

            if (pbds == null)
                throw new ArgumentNullException(nameof(pbds));

            pbds.ForEach(pnl.AddButton);
        }

        /// <summary>
        /// Adds a push button.
        /// </summary>
        /// <param name="pnl"></param>
        /// <param name="pbd"></param>
        /// <param name="pbds"></param>
        /// <returns></returns>
        public static void AddPushButton(this RibbonPanel pnl, PulldownButtonData pbd, List<PushButtonData> pbds)
        {
            if (pnl == null)
                throw new ArgumentNullException(nameof(pnl));

            if (pbd == null)
                throw new ArgumentNullException(nameof(pbd));

            if (pbds == null)
                throw new ArgumentNullException(nameof(pbds));

            if (!(pnl.AddItem(pbd) is PulldownButton pdbtn))
                return;

            pdbtn.ToolTip = pbd.ToolTip;
            pdbtn.LongDescription = pbd.LongDescription;
            pdbtn.LargeImage = pbd.LargeImage;

            foreach (var pbdl in pbds)
            {
                var btn = pdbtn.AddPushButton(pbdl);

                if (btn == null)
                    continue;

                btn.ToolTip = pbdl.ToolTip;
                btn.LongDescription = pbdl.LongDescription;
                btn.LargeImage = pbdl.LargeImage;
            }
        }

        /// <summary>
        /// Adds a push button set.
        /// </summary>
        /// <param name="pnl"></param>
        /// <param name="pbds"></param>
        public static void AddPushButtons(this RibbonPanel pnl, Dictionary<PulldownButtonData, List<PushButtonData>> pbds)
        {
            if (pnl == null)
                throw new ArgumentNullException(nameof(pnl));

            if (pbds == null)
                throw new ArgumentNullException(nameof(pbds));

            foreach (var pbd in pbds)
                pnl.AddPushButton(pbd.Key, pbd.Value);
        }
    }
}