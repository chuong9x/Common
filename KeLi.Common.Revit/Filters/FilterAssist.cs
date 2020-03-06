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
    ///     Filter assist.
    /// </summary>
    public static class FilterAssist
    {
        /// <summary>
        ///     Checkouts all elements in the document.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="type"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public static List<Element> Checkout(this Document doc, FilterType type, ElementId viewId = null)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            var filter = new FilteredElementCollector(doc);

            if (viewId != null)
                filter = new FilteredElementCollector(doc, viewId);

            switch (type)
            {
                case FilterType.Instance:
                    return filter.WhereElementIsNotElementType().ToList();

                case FilterType.Type:
                    return filter.WhereElementIsElementType().ToList();

                case FilterType.All:
                    return filter.ToList();

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        /// <summary>
        ///     Gets planar face list by ray that room cener point to element center point.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elm"></param>
        /// <param name="room"></param>
        /// <param name="doc"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public static List<PlanarFace> GetPlanarFaceList<T>(this T elm, SpatialElement room, Document doc, View3D view) where T: Element
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (room is null)
                throw new ArgumentNullException(nameof(room));

            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (view is null)
                throw new ArgumentNullException(nameof(view));

            var elmCenter = elm.GetBoundingBox(doc).GetBoxCenter();

            var roomCenter = room.GetBoundingBox(doc).GetBoxCenter();

            var direction = (elmCenter - roomCenter).Normalize();

            var elmFilter = new ElementClassFilter(elm.GetType());

            var intersector = new ReferenceIntersector(elmFilter, FindReferenceTarget.Face, view);

            var contexts = intersector.Find(roomCenter, direction);

            var results = new List<PlanarFace>();

            foreach (var context in contexts)
            {
                var reference = context.GetReference();

                var refElm = doc.GetElement(reference.ElementId);

                var face = refElm.GetGeometryObjectFromReference(reference) as Face;

                if (face is PlanarFace planarFace)
                    results.Add(planarFace);
            }

            return results;
        }

        /// <summary>
        ///     Gets the nearest planar face by ray that room cener point to element center point.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elm"></param>
        /// <param name="room"></param>
        /// <param name="doc"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public static PlanarFace GetNearestPlanarFace<T>(this T elm, SpatialElement room, Document doc, View3D view) where T : Element
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (room is null)
                throw new ArgumentNullException(nameof(room));

            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (view is null)
                throw new ArgumentNullException(nameof(view));

            var elmCenter = elm.GetBoundingBox(doc).GetBoxCenter();

            var roomCenter = room.GetBoundingBox(doc).GetBoxCenter();

            var direction = (elmCenter - roomCenter).Normalize();

            var elmFilter = new ElementClassFilter(elm.GetType());

            var intersector = new ReferenceIntersector(elmFilter, FindReferenceTarget.Face, view);

            var context = intersector.FindNearest(roomCenter, direction);

            if (context == null)
                return null;

            var reference = context.GetReference();

            var refElm = doc.GetElement(reference.ElementId);

            var face = refElm.GetGeometryObjectFromReference(reference) as Face;

            if (face is PlanarFace planarFace)
                return planarFace;

            return null;
        }

        /// <summary>
        ///     Gets the specified type of the element list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public static List<T> GetTypeElementList<T>(this Document doc, ElementId viewId = null) where T : Element
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            var filter = new FilteredElementCollector(doc);

            if (viewId != null)
                filter = new FilteredElementCollector(doc, viewId);

            var typeFilter = filter.OfClass(typeof(T)).WhereElementIsElementType();

            return typeFilter.Cast<T>().ToList();
        }

        /// <summary>
        ///     Gets the specified type and category of the element list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="category"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public static List<T> GetTypeElementList<T>(this Document doc, BuiltInCategory category, ElementId viewId = null) where T : Element
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            var filter = new FilteredElementCollector(doc);

            if (viewId != null)
                filter = new FilteredElementCollector(doc, viewId);

            var catgFilter = filter.OfClass(typeof(T)).OfCategory(category);

            return catgFilter.WhereElementIsElementType().Cast<T>().ToList();
        }

        /// <summary>
        ///     Gets the specified type of the element list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public static List<T> GetInstanceElementList<T>(this Document doc, ElementId viewId = null) where T : Element
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            var filter = new FilteredElementCollector(doc);

            if (viewId != null)
                filter = new FilteredElementCollector(doc, viewId);

            var instFilter = filter.OfClass(typeof(T)).WhereElementIsNotElementType();

            return instFilter.Cast<T>().ToList();
        }

        /// <summary>
        ///     Gets the specified type and category of the element list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="category"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public static List<T> GetInstanceElementList<T>(this Document doc, BuiltInCategory category, ElementId viewId = null) where T : Element
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            var filter = new FilteredElementCollector(doc);

            if (viewId != null)
                filter = new FilteredElementCollector(doc, viewId);

            var catgFilter = filter.OfClass(typeof(T)).OfCategory(category);

            var instFilter = catgFilter.WhereElementIsNotElementType();

            return instFilter.Cast<T>().ToList();
        }

        /// <summary>
        ///     Gets the dictionary that geometry info as key and T type element list as value.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="type"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public static Dictionary<int, List<T>> GetGeometryInstancesDict<T>(this Document doc, CalcType type, ElementId viewId = null)where T: Element
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            var elms = doc.GetInstanceElementList<T>(viewId);

            var results = new Dictionary<int, List<T>>();

            foreach (var elm in elms)
            {
                int num;

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

                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }

                if (results.ContainsKey(num))
                    results[num].Add(elm);

                else
                    results.Add(num, new List<T>());
            }

            return results;
        }

        /// <summary>
        ///     Gets the dictionary that geometry info as key and element list as value.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="type"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public static Dictionary<int, List<Element>> GetGeometryInstancesDict(this Document doc, CalcType type, ElementId viewId = null)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            var elms = doc.Checkout(FilterType.Instance, viewId);

            var results = new Dictionary<int, List<Element>>();

            foreach (var elm in elms)
            {
                int num;

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

                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }

                if (results.ContainsKey(num))
                    results[num].Add(elm);

                else
                    results.Add(num, new List<Element>());
            }

            return results;
        }
    }
}