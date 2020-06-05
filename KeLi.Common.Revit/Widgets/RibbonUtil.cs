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
using System.Reflection;
using System.Windows.Media;

using Autodesk.Revit.UI;

namespace KeLi.Common.Revit.Widgets
{
    /// <summary>
    ///     Custom ribbon utility.
    /// </summary>
    public static class RibbonUtil
    {
        /// <summary>
        ///     Creates a button.
        /// </summary>
        /// <param name="pnl"></param>
        /// <param name="pbd"></param>
        /// <returns></returns>
        public static PushButton AddButton(this RibbonPanel pnl, PushButtonData pbd)
        {
            if (pnl is null)
                throw new ArgumentNullException(nameof(pnl));

            if (!(pnl.AddItem(pbd) is PushButton result))
                throw new InvalidCastException();

            result.ToolTip = pbd.ToolTip;

            result.LongDescription = pbd.LongDescription;

            result.LargeImage = pbd.LargeImage;

            return result;
        }

        /// <summary>
        ///     Creates a button list.
        /// </summary>
        /// <param name="pnl"></param>
        /// <param name="pbds"></param>
        /// <returns></returns>
        public static List<PushButton> AddButtonList(this RibbonPanel pnl, IEnumerable<PushButtonData> pbds)
        {
            if (pnl is null)
                throw new ArgumentNullException(nameof(pnl));

            if (pbds is null)
                throw new ArgumentNullException(nameof(pbds));

            if (!pbds.Any())
                throw new TargetParameterCountException(nameof(pbds));

            var results = new List<PushButton>();

            pbds.ToList().ForEach(f => results.Add(pnl.AddButton(f)));

            return results;
        }

        /// <summary>
        ///     Creates a pulldown button.
        /// </summary>
        /// <param name="pnl"></param>
        /// <param name="pbd"></param>
        /// <param name="pbds"></param>
        /// <returns></returns>
        public static PulldownButton AddPushButton(this RibbonPanel pnl, PulldownButtonData pbd, IEnumerable<PushButtonData> pbds)
        {
            if (pnl is null)
                throw new ArgumentNullException(nameof(pnl));

            if (pbd is null)
                throw new ArgumentNullException(nameof(pbd));

            if (pbds is null)
                throw new ArgumentNullException(nameof(pbds));

            if (!pbds.Any())
                throw new TargetParameterCountException(nameof(pbds));

            if (!(pnl.AddItem(pbd) is PulldownButton result))
                throw new InvalidCastException();

            result.ToolTip = pbd.ToolTip;

            result.LongDescription = pbd.LongDescription;

            result.LargeImage = pbd.LargeImage;

            foreach (var pbdl in pbds)
            {
                var btn = result.AddPushButton(pbdl);

                if (btn is null)
                    continue;

                btn.ToolTip = pbdl.ToolTip;

                btn.LongDescription = pbdl.LongDescription;

                btn.LargeImage = pbdl.LargeImage;
            }

            return result;
        }

        /// <summary>
        ///     Creates a split button.
        /// </summary>
        /// <param name="pnl"></param>
        /// <param name="pbd"></param>
        /// <param name="pbds"></param>
        /// <returns></returns>
        public static SplitButton AddPushButton(this RibbonPanel pnl, SplitButtonData pbd, IEnumerable<PushButtonData> pbds)
        {
            if (pnl is null)
                throw new ArgumentNullException(nameof(pnl));

            if (pbd is null)
                throw new ArgumentNullException(nameof(pbd));

            if (pbds is null)
                throw new ArgumentNullException(nameof(pbds));

            if (!pbds.Any())
                throw new TargetParameterCountException(nameof(pbds));

            if (!(pnl.AddItem(pbd) is SplitButton result))
                throw new InvalidCastException();

            result.ToolTip = pbd.ToolTip;

            result.LongDescription = pbd.LongDescription;

            result.LargeImage = pbd.LargeImage;

            foreach (var pbdl in pbds)
            {
                var btn = result.AddPushButton(pbdl);

                if (btn is null)
                    continue;

                btn.ToolTip = pbdl.ToolTip;

                btn.LongDescription = pbdl.LongDescription;

                btn.LargeImage = pbdl.LargeImage;
            }

            return result;
        }

        /// <summary>
        ///     Creates a push button data.
        /// </summary>
        /// <returns></returns>
        public static PushButtonData CreatePushButtonData<T>(string text, ImageSource image, bool isAvailable = false)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text));

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            return new PushButtonData(typeof(T).Name, text, typeof(T).Assembly.Location, typeof(T).FullName)
            {
                LargeImage = image,

                AvailabilityClassName = isAvailable ? typeof(T).FullName : null
            };
        }

        /// <summary>
        ///     Creates a push button data.
        /// </summary>
        /// <returns></returns>
        public static PushButtonData CreatePushButtonData(this IExternalCommand cmd, string text, ImageSource image, bool isAvailable = false)
        {
            if (cmd is null)
                throw new ArgumentNullException(nameof(cmd));

            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text));

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            var type = cmd.GetType();

            return new PushButtonData(type.Name, text, type.Assembly.Location, type.FullName)
            {
                LargeImage = image,

                AvailabilityClassName = isAvailable ? type.FullName : null
            };
        }

        /// <summary>
        ///     Creates a push button data.
        /// </summary>
        /// <returns></returns>
        public static PushButtonData CreatePushButtonData(this Type type, string text, ImageSource image, bool isAvailable = false)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text));

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (type.GetInterfaces().Length == 0 || !type.GetInterfaces().Contains(typeof(IExternalCommand)))
                throw new ArgumentException("Type error. The type isn't a valid command type!");

            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            return new PushButtonData(type.Name, text, type.Assembly.Location, type.FullName)
            {
                LargeImage = image,

                AvailabilityClassName = isAvailable ? type.FullName : null
            };
        }

        /// <summary>
        ///     Creates a pulldown button data.
        /// </summary>
        /// <returns></returns>
        public static PulldownButtonData CreatePulldownButtonData(string text, ImageSource image)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text));

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            return new PulldownButtonData(text, text)
            {
                LargeImage = image
            };
        }

        /// <summary>
        ///     Creates a split button data.
        /// </summary>
        /// <returns></returns>
        public static SplitButtonData CreateSplitButtonData(string text, ImageSource image)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text));

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            return new SplitButtonData(text, text)
            {
                LargeImage = image
            };
        }
    }
}