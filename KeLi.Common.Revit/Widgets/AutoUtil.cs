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
using System.Diagnostics;

using Autodesk.Revit.DB;

using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace KeLi.Common.Revit.Widgets
{
    /// <summary>
    ///     Auto action utility.
    /// </summary>
    public static class AutoUtil
    {
        /// <summary>
        ///     To auto execute transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="act"></param>
        public static void AutoTransaction(this Document doc, Action act)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (act is null)
                throw new ArgumentNullException(nameof(act));

            using (var trans = new Transaction(doc, new StackTrace(true).GetFrame(1).GetMethod().Name))
            {
                trans.Start();

                act.Invoke();

                if (trans.Commit() != TransactionStatus.Committed)
                    trans.RollBack();
            }
        }

        /// <summary>
        ///     To repeat call command.
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public static bool RepeatCommand(this Action act)
        {
            if (act is null)
                throw new ArgumentNullException(nameof(act));

            try
            {
                act.Invoke();

                RepeatCommand(act);
            }
            catch (OperationCanceledException)
            {
                return false;
            }

            return true;
        }
    }
}