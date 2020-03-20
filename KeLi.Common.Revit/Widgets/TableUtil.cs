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
        ///     Converts revit detail view to a DataTable.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(this Document doc, string viewName)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (viewName is null)
                throw new ArgumentNullException(nameof(viewName));

            var view = doc.GetInstanceElementList<ViewSchedule>().FirstOrDefault(f => f.Name == viewName);

            return view.ToDataTable(doc);
        }

        /// <summary>
        ///     Converts revit detail view to a DataTable.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="viewName"></param>
        /// <param name="colNames"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(this Document doc, string viewName, params string[] colNames)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (viewName is null)
                throw new ArgumentNullException(nameof(viewName));

            var view = doc.GetInstanceElementList<ViewSchedule>().FirstOrDefault(f => f.Name == viewName);

            if (view is null)
                throw new NullReferenceException(nameof(view));

            var table = view.GetTableData();

            var body = table.GetSectionData(SectionType.Body);

            var colNum = body.NumberOfColumns;

            var rowNum = body.NumberOfRows;

            var result = new DataTable { TableName = view.GetCellText(SectionType.Header, 0, 0) };

            var colIndexs = new List<int>();

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < colNum; j++)
                {
                    if (colIndexs.Contains(j))
                        continue;

                    var cellText = view.GetCellText(SectionType.Body, i, j);

                    if (colNames.Contains(cellText))
                    {
                        colIndexs.Add(j);

                        result.Columns.Add(cellText);
                    }

                    if (result.Columns.Count == colNames.Length)
                        break;
                }

                if (result.Columns.Count == colNames.Length)
                    break;
            }

            for (var i = 0; i < rowNum; i++)
            {
                var row = result.NewRow();

                for (var j = 0; j < colIndexs.Count; j++)
                    row[j] = view.GetCellText(SectionType.Body, i, colIndexs[j]);

                result.Rows.Add(row);
            }

            return result;
        }

        /// <summary>
        ///     Converts revit detail view to a DataTable.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(this ViewSchedule view, Document doc)
        {
            if (view is null)
                throw new ArgumentNullException(nameof(view));

            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

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
        ///     Converts all revit detail views to DataTable list.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<DataTable> ToDataTableList(this Document doc)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            var views = doc.GetInstanceElementList<ViewSchedule>();

            return views.ToDataTableList(doc);
        }

        /// <summary>
        ///     Converts revit detail views all DataTable list.
        /// </summary>
        /// <param name="viewNames"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<DataTable> ToDataTableList(this IEnumerable<string> viewNames, Document doc)
        {
            if (viewNames is null)
                throw new ArgumentNullException(nameof(viewNames));

            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            var results = new DataSet();

            foreach (var viewName in viewNames)
            {
                var table = doc.ToDataTable(viewName);

                results.Tables.Add(table);
            }

            return results.Tables.Cast<DataTable>().ToList();
        }

        /// <summary>
        ///     Converts revit detail views all DataTable list.
        /// </summary>
        /// <param name="views"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<DataTable> ToDataTableList(this IEnumerable<ViewSchedule> views, Document doc)
        {
            if (views is null)
                throw new ArgumentNullException(nameof(views));

            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            var results = new DataSet();

            foreach (var view in views)
            {
                var table = view.ToDataTable(doc);

                results.Tables.Add(table);
            }

            return results.Tables.Cast<DataTable>().ToList();
        }
    }
}