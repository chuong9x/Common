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
using Autodesk.Revit.DB;
using KeLi.Common.Revit.Geometry;

namespace KeLi.Common.Revit.Filters
{
    /// <summary>
    /// Filter Assist.
    /// </summary>
    public static class FilterAssist
    {
        /// <summary>
        /// Checkouts all elements in the document.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="onlyCurrentView"></param>
        /// <param name="onlyInstance"></param>
        /// <returns></returns>
        public static List<Element> Checkout(this Document doc, bool onlyCurrentView = false, bool onlyInstance = true)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            var baseCollector = onlyCurrentView ? new FilteredElementCollector(doc, doc.ActiveView.Id)
                : new FilteredElementCollector(doc);
            var logicCollector = new LogicalOrFilter(new ElementIsElementTypeFilter(false),
                new ElementIsElementTypeFilter(true));
            var results = baseCollector.WherePasses(logicCollector);

            if (onlyInstance)
                results = results.WhereElementIsNotElementType();

            return results.ToList();
        }

        /// <summary>
        /// Checkouts all elements in the document.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="viewId"></param>
        /// <param name="onlyInstance"></param>
        /// <returns></returns>
        public static List<Element> Checkout(this Document doc, ElementId viewId, bool onlyInstance = true)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            if (viewId == null)
                throw new ArgumentNullException(nameof(viewId));

            var baseCollector = new FilteredElementCollector(doc, viewId);
            var logicCollector = new LogicalOrFilter(new ElementIsElementTypeFilter(false),
                new ElementIsElementTypeFilter(true));
            var results = baseCollector.WherePasses(logicCollector);

            if (onlyInstance)
                results = results.WhereElementIsNotElementType();

            return results.ToList();
        }

        /// <summary>
        /// Gets the specified type of the element set.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="onlyCurrentView"></param>
        /// <param name="onlyInstance"></param>
        /// <returns></returns>
        public static List<T> GetTypeElementList<T>(this Document doc, bool onlyCurrentView = false, bool onlyInstance = true) where T : Element
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            var baseCollector = onlyCurrentView ? new FilteredElementCollector(doc, doc.ActiveView.Id)
                : new FilteredElementCollector(doc);
            var results = baseCollector.OfClass(typeof(T));

            if (onlyInstance)
                results = results.WhereElementIsNotElementType();

            return results.Cast<T>().ToList();
        }

        /// <summary>
        /// Gets the specified type of the element set.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="viewId"></param>
        /// <param name="onlyInstance"></param>
        /// <returns></returns>
        public static List<T> GetTypeElementList<T>(this Document doc, ElementId viewId, bool onlyInstance = true) where T : Element
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            if (viewId == null)
                throw new ArgumentNullException(nameof(viewId));

            var baseCollector = new FilteredElementCollector(doc, viewId);
            var results = baseCollector.OfClass(typeof(T));

            if (onlyInstance)
                results = results.WhereElementIsNotElementType();

            return results.Cast<T>().ToList();
        }

        /// <summary>
        /// Gets the specified category of the element set.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="category"></param>
        /// <param name="onlyCurrentView"></param>
        /// <param name="onlyInstance"></param>
        /// <returns></returns>
        public static List<Element> GetCategoryElementList(this Document doc, BuiltInCategory category, bool onlyCurrentView = false, bool onlyInstance = true)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            var baseCollector = onlyCurrentView ? new FilteredElementCollector(doc, doc.ActiveView.Id)
                : new FilteredElementCollector(doc);
            var results = baseCollector.OfCategory(category);

            if (onlyInstance)
                results = results.WhereElementIsNotElementType();

            return results.ToList();
        }

        /// <summary>
        /// Gets the specified category of the element set.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="category"></param>
        /// <param name="viewId"></param>
        /// <param name="onlyInstance"></param>
        /// <returns></returns>
        public static List<Element> GetCategoryElementList(this Document doc, BuiltInCategory category, ElementId viewId, bool onlyInstance = true)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            if (viewId == null)
                throw new ArgumentNullException(nameof(viewId));

            var baseCollector = new FilteredElementCollector(doc, viewId);
            var results = baseCollector.OfCategory(category);

            if (onlyInstance)
                results = results.WhereElementIsNotElementType();

            return results.ToList();
        }

        /// <summary>
        /// Gets the specified type and category of the element set.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="category"></param>
        /// <param name="onlyCurrentView"></param>
        /// <param name="onlyInstance"></param>
        /// <returns></returns>
        public static List<T> GetTypeElementList<T>(this Document doc, BuiltInCategory category, bool onlyCurrentView = false, bool onlyInstance = true) where T : Element
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            var baseCollector = onlyCurrentView ? new FilteredElementCollector(doc, doc.ActiveView.Id)
                : new FilteredElementCollector(doc);
            var results = baseCollector.OfClass(typeof(T)).OfCategory(category);

            if (onlyInstance)
                results = results.WhereElementIsNotElementType();

            return results.Cast<T>().ToList();
        }

        /// <summary>
        /// Gets the specified type and category of the element set.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="category"></param>
        /// <param name="viewId"></param>
        /// <param name="onlyInstance"></param>
        /// <returns></returns>
        public static List<T> GetTypeElementList<T>(this Document doc, BuiltInCategory category, ElementId viewId, bool onlyInstance = true) where T : Element
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            if (viewId == null)
                throw new ArgumentNullException(nameof(viewId));

            var baseCollector = new FilteredElementCollector(doc, viewId);
            var results = baseCollector.OfClass(typeof(T)).OfCategory(category);

            if (onlyInstance)
                results = results.WhereElementIsNotElementType();

            return results.Cast<T>().ToList();
        }

        /// <summary>
        /// Gets the element set that filter the max number of points.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="maxNum"></param>
        /// <param name="moreThan"></param>
        /// <param name="type"></param>
        /// <param name="onlyCurrentView"></param>
        /// <returns></returns>
        public static List<Element> GetElementList(this Document doc, CalcType type, int maxNum, bool moreThan, bool onlyCurrentView = false)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            var elms = doc.Checkout(onlyCurrentView);
            var results = new List<Element>();
            var num = 0;

            foreach (var elm in elms)
            {
                switch (type)
                {
                    case CalcType.FaceNum:
                        num = elm.GetFaceList().Count;
                        break;

                    case CalcType.FacePointNum:
                        num = elm.GetFacePointList().Count;
                        break;

                    case CalcType.SolidPointNum:
                        num = elm.GetSolidPointList().Count;
                        break;
                }

                if (moreThan && num <= maxNum)
                    continue;

                if (!moreThan && num > maxNum)
                    continue;

                results.Add(elm);
            }

            return results;
        }

        /// <summary>
        /// Gets the element set that filter the max number of points.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="maxNum"></param>
        /// <param name="moreThan"></param>
        /// <param name="type"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public static List<Element> GetElementList(this Document doc, CalcType type, int maxNum, bool moreThan, ElementId viewId)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            if (viewId == null)
                throw new ArgumentNullException(nameof(viewId));

            var elms = doc.Checkout(viewId);
            var results = new List<Element>();
            var num = 0;

            foreach (var elm in elms)
            {
                switch (type)
                {
                    case CalcType.FaceNum:
                        num = elm.GetFaceList().Count;
                        break;

                    case CalcType.FacePointNum:
                        num = elm.GetFacePointList().Count;
                        break;

                    case CalcType.SolidPointNum:
                        num = elm.GetSolidPointList().Count;
                        break;
                }

                if (moreThan && num <= maxNum)
                    continue;

                if (!moreThan && num > maxNum)
                    continue;

                results.Add(elm);
            }

            return results;
        }

        /// <summary>
        /// Gets the max number of points element and the number.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="type"></param>
        /// <param name="onlyCurrentView"></param>
        /// <returns></returns>
        public static (Element, int) GetMaxElementPair(this Document doc, CalcType type, bool onlyCurrentView = false)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            var elms = doc.Checkout(onlyCurrentView);
            var maxElm = default(Element);
            var maxNum = int.MinValue;
            var num = 0;

            foreach (var elm in elms)
            {
                switch (type)
                {
                    case CalcType.FaceNum:
                        num = elm.GetFaceList().Count;
                        break;

                    case CalcType.FacePointNum:
                        num = elm.GetFacePointList().Count;
                        break;

                    case CalcType.SolidPointNum:
                        num = elm.GetSolidPointList().Count;
                        break;
                }

                if (num <= maxNum)
                    continue;

                maxNum = num;
                maxElm = elm;
            }

            return (maxElm, maxNum);
        }

        /// <summary>
        /// Gets the max number of points element and the number.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="type"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public static (Element, int) GetMaxElementPair(this Document doc, CalcType type, ElementId viewId)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            if (viewId == null)
                throw new ArgumentNullException(nameof(viewId));

            var elms = doc.Checkout(viewId);
            var maxElm = default(Element);
            var maxNum = int.MinValue;
            var num = 0;

            foreach (var elm in elms)
            {
                switch (type)
                {
                    case CalcType.FaceNum:
                        num = elm.GetFaceList().Count;
                        break;

                    case CalcType.FacePointNum:
                        num = elm.GetFacePointList().Count;
                        break;

                    case CalcType.SolidPointNum:
                        num = elm.GetSolidPointList().Count;
                        break;
                }

                if (num <= maxNum)
                    continue;

                maxNum = num;
                maxElm = elm;
            }

            return (maxElm, maxNum);
        }
    }
}
