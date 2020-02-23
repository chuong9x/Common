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
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using KeLi.Common.Converter.Collections;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace KeLi.Common.Drive.Excel
{
    /// <summary>
    ///     A excel assist.
    /// </summary>
    public static class ExcelAssist
    {
        /// <summary>
        ///     Reads the excel to a two dimension array.
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public static object[,] As2DArray(this ExcelParameter parm)
        {
            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            using (var excel = new ExcelPackage(parm.FilePath))
            {
                var sheets = excel.Workbook.Worksheets;
                var sheet = (parm.SheetName is null ? sheets.FirstOrDefault() : sheets[parm.SheetName]) ?? sheets.FirstOrDefault();

                if (!(sheet?.Cells.Value is object[,]))
                    return new object[0, 0];

                return (object[,])sheet.Cells.Value;
            }
        }

        /// <summary>
        ///     Reads the excel to a cross array.
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public static object[][] AsCrossArray(this ExcelParameter parm)
        {
            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            return parm.As2DArray().Convert();
        }

        /// <summary>
        ///     Reads the excel to a DataTable.
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public static DataTable AsDataTable(this ExcelParameter parm)
        {
            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            var results = new DataTable();

            using (var excel = new ExcelPackage(parm.FilePath))
            {
                var sheets = excel.Workbook.Worksheets;
                var sheet = (parm.SheetName is null ? sheets.FirstOrDefault() : sheets[parm.SheetName]) ?? sheets.FirstOrDefault();

                if (!(sheet?.Cells.Value is object[,] cells))
                    return new DataTable();

                for (var j = parm.ColumnIndex; j < sheet.Dimension.Columns; j++)
                    results.Columns.Add(new DataColumn(cells[0, j]?.ToString()));

                for (var i = parm.RowIndex; i < sheet.Dimension.Rows; i++)
                    for (var j = parm.ColumnIndex; j < sheet.Dimension.Columns; j++)
                        results.Rows[i - parm.RowIndex][j] = cells[i + parm.RowIndex, j];
            }

            return results;
        }

        /// <summary>
        ///     Reads the excel to a List.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parm"></param>
        /// <returns></returns>
        public static List<T> AsList<T>(this ExcelParameter parm)
        {
            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            var results = new List<T>();
            var ps = typeof(T).GetProperties();

            using (var excel = new ExcelPackage(parm.FilePath))
            {
                var sheets = excel.Workbook.Worksheets;
                var sheet = (parm.SheetName is null ? sheets.FirstOrDefault() : sheets[parm.SheetName]) ?? sheets.FirstOrDefault();

                if (!(sheet?.Cells.Value is object[,] cells))
                    return new List<T>();

                for (var i = parm.RowIndex; i < sheet.Dimension.Rows; i++)
                {
                    var obj = (T)Activator.CreateInstance(typeof(T));

                    for (var j = parm.ColumnIndex; j < typeof(T).GetProperties().Length + parm.ColumnIndex; j++)
                    {
                        var columnName = cells[0, j]?.ToString();
                        var pls = ps.Where(w => w.GetDcrp().Equals(columnName) || w.Name.Equals(cells[parm.RowIndex, j]));

                        foreach (var p in pls)
                        {
                            var v1 = Enum.Parse(p.PropertyType, cells[i, j].ToString());
                            var v2 = Convert.ChangeType(cells[i, j], p.PropertyType);
                            var val = p.PropertyType.IsEnum ? v1 : v2;

                            p.SetValue(obj, cells[i, j] != DBNull.Value ? val : null, null);
                            break;
                        }
                    }

                    results.Add(obj);
                }
            }

            return results;
        }

        /// <summary>
        ///     Writes the IEnumerable to the excel.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <param name="parm"></param>
        /// <param name="createHeader"></param>
        public static void ToExcel<T>(this ExcelParameter parm, IEnumerable<T> objs, bool createHeader = true)
        {
            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            if (objs is null)
                throw new ArgumentNullException(nameof(objs));

            // If exists data, auto width setting will throw exception.
            if (parm.FilePath.Exists)
                File.Delete(parm.FilePath.FullName);

            File.Copy(parm.TemplatePath.FullName, parm.FilePath.FullName);

            // Epplus dll write excel file that column index from 1 to end column index and row index from 0 to end row index.
            parm.ColumnIndex += 1;

            var excel = parm.GetExcelPackage(out var sheet);
            var ps = typeof(T).GetProperties();

            // The titlt row.
            if (createHeader)
                for (var i = 0; i < ps.Length; i++)
                    sheet.Cells[parm.RowIndex, i + parm.ColumnIndex].Value = ps[i].GetDcrp();

            var tmpObjs = objs.ToList();

            // The content row.
            for (var i = 0; i < tmpObjs.Count; i++)
                for (var j = 0; j < ps.Length; j++)
                    sheet.Cells[i + parm.RowIndex + 1, j + parm.ColumnIndex].Value = ps[j].GetValue(tmpObjs[i]);

            sheet.SetExcelStyle();
            excel.Save();
        }

        /// <summary>
        ///     Writes the cross array to a excel.
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static ExcelPackage ToExcel(this ExcelParameter parm, object[][] table)
        {
            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            if (table is null)
                throw new ArgumentNullException(nameof(table));

            // If exists, auto width setting will throw exception.
            if (parm.FilePath.Exists)
                File.Delete(parm.FilePath.FullName);

            File.Copy(parm.TemplatePath.FullName, parm.FilePath.FullName);

            // Epplus dll write excel file that column index from 1 to end column index and row index from 0 to end row index.
            parm.ColumnIndex += 1;

            var excel = parm.GetExcelPackage(out var sheet);

            for (var i = 0; i < table.GetLength(0); i++)
                for (var j = 0; j < table[i].Length; j++)
                    sheet.Cells[i + parm.RowIndex, j + parm.ColumnIndex].Value = table[i][j];

            sheet.SetExcelStyle();
            excel.Save();

            return excel;
        }

        /// <summary>
        ///     Writes the two dimension array to a excel.
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static ExcelPackage ToExcel(this ExcelParameter parm, object[,] table)
        {
            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            if (table is null)
                throw new ArgumentNullException(nameof(table));

            // If exists, auto width setting will throw exception.
            if (parm.FilePath.Exists)
                File.Delete(parm.FilePath.FullName);

            File.Copy(parm.TemplatePath.FullName, parm.FilePath.FullName);

            // Epplus dll write excel file that column index from 1 to end column index and row index from 0 to end row index.
            parm.ColumnIndex += 1;

            var excel = parm.GetExcelPackage(out var sheet);

            for (var i = 0; i < table.GetLength(0); i++)
                for (var j = 0; j < table.GetLength(1); j++)
                    sheet.Cells[i + parm.RowIndex, j + parm.ColumnIndex].Value = table[i, j];

            sheet.SetExcelStyle();
            excel.Save();

            return excel;
        }

        /// <summary>
        ///     Writes the DataTable to a excel.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="parm"></param>
        /// <param name="createHeader"></param>
        public static ExcelPackage ToExcel(this ExcelParameter parm, DataTable table, bool createHeader = true)
        {
            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            if (table is null)
                throw new ArgumentNullException(nameof(table));

            // If exists, auto width setting will throw exception.
            if (parm.FilePath.Exists)
                File.Delete(parm.FilePath.FullName);

            File.Copy(parm.TemplatePath.FullName, parm.FilePath.FullName);

            // Epplus dll write excel file that column index from 1 to end column index and row index from 0 to end row index.
            parm.ColumnIndex += 1;

            var excel = parm.GetExcelPackage(out var sheet);
            var columns = table.Columns.Cast<DataColumn>().ToList();

            // The titlt row.
            for (var i = 0; createHeader && i < columns.Count; i++)
                sheet.Cells[parm.RowIndex, i + parm.ColumnIndex].Value = columns[i].ColumnName;

            // The cotent row.
            for (var i = 0; i < table.Rows.Count; i++)
                for (var j = 0; j < columns.Count; j++)
                    sheet.Cells[i + parm.RowIndex + 1, j + parm.ColumnIndex].Value = table.Rows[i][columns[j].ColumnName];

            sheet.SetExcelStyle();
            excel.Save();

            return excel;
        }

        /// <summary>
        ///     Sets custom style.
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="action"></param>
        /// <param name="parm"></param>
        public static void SetExcelStyle(this ExcelPackage excel, Action<ExcelWorksheet> action, ExcelParameter parm)
        {
            if (excel is null)
                throw new ArgumentNullException(nameof(excel));

            if (action is null)
                throw new ArgumentNullException(nameof(action));

            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            action(excel.Workbook.Worksheets[parm.SheetName]);
            excel.Save();
        }

        /// <summary>
        ///     Gets the excel object.
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static ExcelPackage GetExcelPackage(this ExcelParameter parm, out ExcelWorksheet sheet)
        {
            if (parm is null)
                throw new ArgumentNullException(nameof(parm));

            var result = new ExcelPackage(parm.FilePath);
            var sheets = result.Workbook.Worksheets;

            sheet = sheets.FirstOrDefault(f => f.Name.ToLower() == parm.SheetName.ToLower());
            sheet = sheet != null ? sheets[parm.SheetName] : sheets.Add(parm.SheetName);

            return result;
        }

        /// <summary>
        ///     Gets the property's description attribute value.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string GetDcrp(this PropertyInfo p)
        {
            if (p is null)
                throw new ArgumentNullException(nameof(p));

            var objs = p.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // To throw not exception, must return empty string.
            return objs.Length == 0 ? string.Empty : (objs[0] as DescriptionAttribute)?.Description;
        }

        /// <summary>
        ///     Gets the property's span attribute value.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static int GetSpan(this PropertyInfo p)
        {
            if (p is null)
                throw new ArgumentNullException(nameof(p));

            var objs = p.GetCustomAttributes(typeof(SpanAttribute), false);

            if (objs.Length == 0)
                return 1;

            if (objs[0] is SpanAttribute attr)
                return objs.Length == 0 ? 1 : attr.ColumnSpan;

            return 1;
        }

        /// <summary>
        ///     Gets the property's reference attribute value.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string GetReference(this PropertyInfo p)
        {
            if (p is null)
                throw new ArgumentNullException(nameof(p));

            var objs = p.GetCustomAttributes(typeof(ReferenceAttribute), false);

            if (objs.Length == 0)
                return string.Empty;

            if (objs[0] is ReferenceAttribute attr)
                return objs.Length == 0 ? string.Empty : attr.ColumnName;

            return string.Empty;
        }

        /// <summary>
        ///     Gets merged range cell value.
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static string GetMegerValue(this ExcelWorksheet worksheet, int row, int column)
        {
            if (worksheet is null)
                throw new ArgumentNullException(nameof(worksheet));

            var rangeStr = worksheet.MergedCells[row, column];
            var excelRange = worksheet.Cells;
            var cellVal = excelRange[row, column].Value;

            if (rangeStr is null)
                return cellVal?.ToString();

            var startCell = new ExcelAddress(rangeStr).Start;

            return excelRange[startCell.Row, startCell.Column].Value?.ToString() ?? string.Empty;
        }

        /// <summary>
        ///     Sets the excel style.
        /// </summary>
        /// <param name="worksheet"></param>
        public static void SetExcelStyle(this ExcelWorksheet worksheet)
        {
            if (worksheet is null)
                throw new ArgumentNullException(nameof(worksheet));

            worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells.AutoFitColumns();
        }
    }
}