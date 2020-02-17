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
using System.Data;
using System.Linq;
using Autodesk.Revit.DB;
using KeLi.Common.Revit.Filters;

namespace KeLi.Common.Revit.Widgets
{
    /// <summary>
    ///     Table utility.
    /// </summary>
    public static class TableUtil
    {
        /// <summary>
        ///     Gets revit detail list's DataTable.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(this Document doc, string tableName)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            if (tableName == null)
                throw new ArgumentNullException(nameof(tableName));

            var view = doc.GetInstanceElementList<ViewSchedule>().FirstOrDefault(f => f.Name == tableName);

            return doc.GetDataTable(view);
        }

        /// <summary>
        ///     Gets revit detail list's DataTable.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(this Document doc, ViewSchedule view)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            if (view == null)
                throw new ArgumentNullException(nameof(view));

            if (view == null)
                throw new NullReferenceException(nameof(view));

            var table = view.GetTableData();
            var body = table.GetSectionData(SectionType.Body);
            var colNum = body.NumberOfColumns;
            var rowNum = body.NumberOfRows;
            var result = new DataTable { TableName = view.GetCellText(SectionType.Header, 0, 0) };

            for (var i = 0; i < colNum; i++)
                result.Columns.Add();

            for (var i = 0; i < rowNum; i++)
            {
                var row = result.NewRow();

                for (var j = 0; j < colNum; j++)
                    row[j] = view.GetCellText(SectionType.Body, i, j);

                result.Rows.Add(row);
            }

            return result;
        }

        /// <summary>
        /// Gets revit detail list's all DataTable set.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(this Document doc)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            var views = doc.GetInstanceElementList<ViewSchedule>();

            return doc.GetDataSet(views);
        }

        /// <summary>
        /// Gets revit detail list's all DataTable set.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="views"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(this Document doc, IEnumerable<ViewSchedule> views)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            if (views == null)
                throw new ArgumentNullException(nameof(views));

            var results = new DataSet();

            foreach (var view in views)
            {
                var table = doc.GetDataTable(view);

                results.Tables.Add(table);
            }

            return results;
        }
    }
}