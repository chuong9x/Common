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
     |  |              Creation Time: 06/05/2020 12:05:00 PM |  |  |     |         |      |
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

using Autodesk.Revit.DB;

namespace KeLi.Common.Revit.Widgets
{
    /// <summary>
    ///     Element deletion utility.
    /// </summary>
    public static class DeletionUtil
    {
        /// <summary>
        ///     Deletes element list.
        /// </summary>
        /// <param name="elms"></param>
        public static void DeleteElementList(params Element[] elms)
        {
            if (elms is null)
                throw new ArgumentNullException(nameof(elms));

            elms.DeleteElementList();
        }

        /// <summary>
        ///     Deletes element list.
        /// </summary>
        /// <param name="elms"></param>
        public static void DeleteElementList(this IEnumerable<Element> elms)
        {
            if (elms is null)
                throw new ArgumentNullException(nameof(elms));

            elms.ToList().ForEach(DeleteElement);
        }

        /// <summary>
        ///     Deletes element list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ids"></param>
        public static void DeleteElementList(this Document doc, params ElementId[] ids)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (ids is null)
                throw new ArgumentNullException(nameof(ids));

            ids.ToList().ForEach(doc.DeleteElement);
        }

        /// <summary>
        ///     Deletes element list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ids"></param>
        public static void DeleteElementList(this Document doc, IEnumerable<ElementId> ids)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (ids is null)
                throw new ArgumentNullException(nameof(ids));

            ids.ToList().ForEach(doc.DeleteElement);
        }

        /// <summary>
        ///     Deletes element.
        /// </summary>
        /// <param name="elm"></param>
        public static void DeleteElement(this Element elm)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (!elm.IsValidObject)
                return;

            if (elm.Id.IntegerValue == -1)
                return;

            elm.Document.Delete(elm.Id);
        }

        /// <summary>
        ///     Deletes element.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="id"></param>
        public static void DeleteElement(this Document doc, ElementId id)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (id is null)
                throw new ArgumentNullException(nameof(id));

            if (id.IntegerValue == -1)
                return;

            var elm = doc.GetElement(id);

            if (elm == null || !elm.IsValidObject)
                return;

            doc.Delete(id);
        }

        /// <summary>
        ///     Deletes element.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="doc"></param>
        public static void DeleteElement(this ElementId id, Document doc)
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id));

            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            doc.DeleteElement(id);
        }
    }
}
