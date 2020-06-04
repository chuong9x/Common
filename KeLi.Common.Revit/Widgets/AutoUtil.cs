﻿/*
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
using System.Diagnostics;
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

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
        /// <param name="action"></param>
        public static bool AutoTransaction(this Document doc, Action action)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (action is null)
                throw new ArgumentNullException(nameof(action));

            using (var trans = new Transaction(doc, new StackTrace(true).GetFrame(1).GetMethod().Name))
            {
                trans.Start();

                action.Invoke();

                if (trans.Commit() == TransactionStatus.Committed)
                    return true;

                if (trans.GetStatus() == TransactionStatus.RolledBack)
                    return false;

                if (trans.GetStatus() != TransactionStatus.RolledBack)
                {
                    trans.RollBack();

                    return false;
                }

                return false;
            }
        }

        /// <summary>
        ///     To auto execute transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="actions"></param>
        public static bool AutoTransaction(this Document doc, params Action[] actions)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (actions is null)
                throw new ArgumentNullException(nameof(actions));

            return doc.AutoTransaction(() => actions.ToList().ForEach(f => f?.Invoke()));
        }

        /// <summary>
        ///     To auto execute transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="actions"></param>
        public static bool AutoTransaction(this Document doc, IEnumerable<Action> actions)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (actions is null)
                throw new ArgumentNullException(nameof(actions));

            return doc.AutoTransaction(() => actions.ToList().ForEach(f => f?.Invoke()));
        }

        /// <summary>
        ///     To auto execute transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="func"></param>
        public static T AutoTransaction<T>(this Document doc, Func<T> func) where T : Element
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (func is null)
                throw new ArgumentNullException(nameof(func));

            T result = null;

            doc.AutoTransaction(() => result = func.Invoke());

            return result;
        }

        /// <summary>
        ///     To auto execute transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="funcs"></param>
        public static List<T> AutoTransaction<T>(this Document doc, params Func<T>[] funcs) where T : Element
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (funcs is null)
                throw new ArgumentNullException(nameof(funcs));

            var results = new List<T>();

            doc.AutoTransaction(() => results = funcs.Select(s => s.Invoke()).ToList());

            return results;
        }

        /// <summary>
        ///     To auto execute transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="funcs"></param>
        public static List<T> AutoTransaction<T>(this Document doc, IEnumerable<Func<T>> funcs) where T : Element
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (funcs is null)
                throw new ArgumentNullException(nameof(funcs));

            var results = new List<T>();

            doc.AutoTransaction(() => results = funcs.Select(s => s.Invoke()).ToList());

            return results;
        }

        /// <summary>
        ///     To auto execute transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="func"></param>
        public static List<T> AutoTransaction<T>(this Document doc, Func<IEnumerable<T>> func) where T : Element
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (func is null)
                throw new ArgumentNullException(nameof(func));

            var results = new List<T>();

            doc.AutoTransaction(() => results = func.Invoke().ToList());

            return results;
        }

        /// <summary>
        ///     To auto execute transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="funcs"></param>
        public static List<List<T>> AutoTransaction<T>(this Document doc, IEnumerable<Func<IEnumerable<T>>> funcs) where T : Element
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (funcs is null)
                throw new ArgumentNullException(nameof(funcs));

            var results = new List<List<T>>();

            doc.AutoTransaction(() => results = funcs.Select(s => s.Invoke().ToList()).ToList());

            return results;
        }

        /// <summary>
        ///     To repeat call command.
        /// </summary>
        /// <param name="uidoc"></param>
        /// <param name="func"></param>
        /// <param name="flag"></param>
        /// <param name="ignoreEsc"></param>
        /// <returns></returns>
        public static bool RepeatCommand(this UIDocument uidoc, Func<bool> func, bool flag = true, bool ignoreEsc = true)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            try
            {
                if (flag && func.Invoke())
                    return RepeatCommand(uidoc, func);
            }
            catch (OperationCanceledException)
            {
                if (ignoreEsc && func.Invoke())
                    return RepeatCommand(uidoc, func);
            }

            return false;
        }

        /// <summary>
        ///     To repeat call command.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="func"></param>
        /// <param name="flag"></param>
        /// <param name="ignoreEsc"></param>
        /// <returns></returns>
        public static bool RepeatCommand(this Document doc, Func<bool> func, bool flag = true, bool ignoreEsc = true)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            try
            {
                if (flag && func.Invoke())
                    return RepeatCommand(doc, func);
            }
            catch (OperationCanceledException)
            {
                if (ignoreEsc && func.Invoke())
                    return RepeatCommand(doc, func);
            }

            return false;
        }

        /// <summary>
        ///     Deletes element list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="elms"></param>
        public static void DeleteElementList(this Document doc, List<Element> elms)
        {
            // It may be deleted.
            elms = elms.Where(w => w != null && w.IsValidObject).ToList();

            var ids = elms.Select(s => s.Id).ToList();

            doc.DeleteElementList(ids);
        }

        /// <summary>
        ///     Deletes element list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ids"></param>
        public static void DeleteElementList(this Document doc, List<ElementId> ids)
        {
            AutoTransaction(doc, () =>
            {
                foreach (var id in ids)
                {
                    if (id == null || id.IntegerValue == -1)
                        continue;

                    if (doc.GetElement(id) != null)
                        doc.Delete(id);
                }
            });
        }
    }
}


namespace KeLi.RevitDev.App.Commands
{
}