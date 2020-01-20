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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using O2S.Components.PDFRender4NET;

namespace KeLi.Common.Drive.Pdf
{
    /// <summary>
    /// A pdf converter.
    /// </summary>
    public static class PdfConverter
    {
        /// <summary>
        /// Convers pdf to images.
        /// </summary>
        /// <param name="param"></param>
        /// <exception cref="FileNotFoundException"></exception>
        public static List<string> ToImageList(this PdfParam param)
        {
            if (param == null)
                throw new ArgumentNullException(nameof(param));

            var results = new List<string>();
            var pdfFile = PDFFile.Open(param.PdfPath.FullName);

            if (param.StartPage <= 0)
                param.StartPage = 1;

            if (param.EndPage > pdfFile.PageCount)
                param.EndPage = pdfFile.PageCount;

            if (param.StartPage > param.EndPage)
            {
                param.StartPage = param.EndPage;
                param.EndPage = param.StartPage;
            }

            for (var i = param.StartPage; i <= param.EndPage; i++)
            {
                var withNum = i.ToString().PadLeft(pdfFile.PageCount.ToString().Length, '0');

                withNum = param.StartPage == param.EndPage ? null : "_" + withNum;

                var imgPage = pdfFile.GetPageImage(i - 1, 56 * param.Resolution);
                var filePath = Path.Combine(param.PdfPath.DirectoryName ?? throw new InvalidOperationException(), param.ImgName + withNum + "." + param.Format.ToString().ToLower());

                results.Add(filePath);
                imgPage.Save(filePath, param.Format);
                imgPage.Dispose();
            }

            pdfFile.Dispose();

            return results;
        }

        /// <summary>
        /// Gets file page's size.
        /// </summary>
        /// <param name="pdfPath"></param>
        /// <returns></returns>
        public static Size GetPdfSize(this FileInfo pdfPath)
        {
            if (pdfPath == null)
                throw new ArgumentNullException(nameof(pdfPath));

            var reader = new PdfReader(pdfPath.FullName);
            var width = reader.GetPageSizeWithRotation(1).Width;
            var height = reader.GetPageSizeWithRotation(1).Height;

            reader.Close();

            return new Size(Convert.ToInt32(width), Convert.ToInt32(height));
        }

        /// <summary>
        /// Splits the pdf file to the multi pdf files.
        /// </summary>
        /// <param name="sourcePdf"></param>
        public static List<FileInfo> SplitedPdfList(FileInfo sourcePdf)
        {
            if (sourcePdf == null)
                throw new ArgumentNullException(nameof(sourcePdf));

            var results = new List<FileInfo>();
            var reader = new PdfReader(sourcePdf.FullName);
            var titled = GetPageMark(reader);

            if (reader.NumberOfPages == 1)
            {
                results = new List<FileInfo> { sourcePdf };
                reader.Close();

                return results;
            }

            for (var i = 1; i <= reader.NumberOfPages; i++)
            {
                var targetPath = Path.GetDirectoryName(sourcePdf.FullName);
                var targetName = Path.GetFileNameWithoutExtension(sourcePdf.FullName);

                if (titled.ContainsKey(i))
                    targetName += "_" + titled[i] + Path.GetExtension(sourcePdf.FullName);
                else
                    targetName += "_" + i + Path.GetExtension(sourcePdf.FullName);

                var targetPdf = new FileInfo(Path.Combine(targetPath ?? throw new InvalidOperationException(), targetName));

                CopyPdf(sourcePdf, targetPdf, i, i);
                results.Add(targetPdf);
            }
            reader.Close();

            return results;
        }

        /// <summary>
        /// Copys the pdf range content to the new pdf file.
        /// </summary>
        /// <param name="sourcePdf"></param>
        /// <param name="targetPdf"></param>
        /// <param name="startPage"></param>
        /// <param name="endPage"></param>
        public static void CopyPdf(FileInfo sourcePdf, FileInfo targetPdf, int startPage, int endPage)
        {
            if (sourcePdf == null)
                throw new ArgumentNullException(nameof(sourcePdf));

            if (targetPdf == null)
                throw new ArgumentNullException(nameof(targetPdf));

            var reader = new PdfReader(sourcePdf.FullName);
            var doc = new Document(reader.GetPageSizeWithRotation(startPage));
            var writer = PdfWriter.GetInstance(doc, new FileStream(targetPdf.FullName, FileMode.Create));

            doc.Open();

            var content = writer.DirectContent;

            for (var i = startPage - 1; i < endPage; i++)
            {
                doc.SetPageSize(reader.GetPageSizeWithRotation(startPage));
                doc.NewPage();

                var page = writer.GetImportedPage(reader, startPage);
                var rotation = reader.GetPageRotation(startPage);

                if (rotation != 90 && rotation != 270)
                    content.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                else
                {
                    switch (rotation)
                    {
                        case 90:
                            content.AddTemplate(page, 0, -1f, 1f, 0, 0,
                                reader.GetPageSizeWithRotation(startPage).Height);
                            break;

                        case 270:
                            content.AddTemplate(page, 0, 1.0F, -1.0F, 0,
                                reader.GetPageSizeWithRotation(startPage).Width, 0);
                            break;
                    }
                }

                startPage++;
            }

            doc.Close();
            reader.Close();
        }

        /// <summary>
        /// Extracts the pdf range content to new pdf file.
        /// </summary>
        /// <param name="sourcePdf"></param>
        /// <param name="targetPdf"></param>
        /// <param name="startPage"></param>
        /// <param name="endPage"></param>
        public static void ExtractPdf(FileInfo sourcePdf, FileInfo targetPdf, int startPage, int endPage)
        {
            if (sourcePdf == null)
                throw new ArgumentNullException(nameof(sourcePdf));

            if (targetPdf == null)
                throw new ArgumentNullException(nameof(targetPdf));

            var reader = new PdfReader(sourcePdf.FullName);
            var doc = new Document(reader.GetPageSizeWithRotation(startPage));
            var copy = new PdfCopy(doc, new FileStream(targetPdf.FullName, FileMode.Create));

            doc.Open();

            for (var i = startPage; i <= endPage; i++)
            {
                var importPage = copy.GetImportedPage(reader, i);

                copy.AddPage(importPage);
            }

            doc.Close();
            reader.Close();
        }

        /// <summary>
        /// Extracts the pdf pages content to new pdf file.
        /// </summary>
        /// <param name="sourcePdf"></param>
        /// <param name="targetPdf"></param>
        /// <param name="extractPages"></param>
        public static void ExtractPdf(FileInfo sourcePdf, FileInfo targetPdf, IEnumerable<int> extractPages)
        {
            if (sourcePdf == null)
                throw new ArgumentNullException(nameof(sourcePdf));

            if (targetPdf == null)
                throw new ArgumentNullException(nameof(targetPdf));

            if (extractPages == null)
                throw new ArgumentNullException(nameof(extractPages));

            var tmpExtractPages = extractPages.ToList();
            var reader = new PdfReader(sourcePdf.FullName);
            var doc = new Document(reader.GetPageSizeWithRotation(tmpExtractPages[0]));
            var copy = new PdfCopy(doc, new FileStream(targetPdf.FullName, FileMode.Create));

            doc.Open();

            foreach (var pageNumber in extractPages)
                copy.AddPage(copy.GetImportedPage(reader, pageNumber));

            doc.Close();
            reader.Close();
        }

        /// <summary>
        /// Gets the pdf page bookmark.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static Dictionary<int, string> GetPageMark(PdfReader reader)
        {
            var marks = SimpleBookmark.GetBookmark(reader);
            var results = new Dictionary<int, string>();

            foreach (Hashtable mark in marks)
            {
                var title = string.Empty;
                var page = 0;

                foreach (DictionaryEntry kv in mark)
                {
                    switch (kv.Key.ToString())
                    {
                        case "Action":
                            continue;
                        case "Title":
                            title = kv.Value.ToString();
                            break;

                        case "Page":
                            page = Convert.ToInt32(kv.Value.ToString().Split(' ')[0]);
                            break;
                    }
                }

                results.Add(page, title);
            }

            return results;
        }
    }
}