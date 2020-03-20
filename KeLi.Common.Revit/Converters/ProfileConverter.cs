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

using KeLi.Common.Revit.Relations;

namespace KeLi.Common.Revit.Converters
{
    /// <summary>
    ///     Pprofile converter.
    /// </summary>
    public static class ProfileConverter
    {
        /// <summary>
        ///     Convers the space Curve to the plane Line.
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static Line ToPlaneLine(this Curve curve)
        {
            if (curve is null)
                throw new ArgumentNullException(nameof(curve));

            var p1 = curve.GetEndPoint(0).ToPlanePoint();

            var p2 = curve.GetEndPoint(1).ToPlanePoint();

            return Line.CreateBound(p1, p2);
        }

        /// <summary>
        ///     Convers the space XYZ to the plane XYZ.
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static XYZ ToPlanePoint(this XYZ pt)
        {
            if (pt is null)
                throw new ArgumentNullException(nameof(pt));

            return new XYZ(pt.X, pt.Y, 0);
        }

        /// <summary>
        ///     Converts the Reference set to the ReferenceArray.
        /// </summary>
        /// <param name="references"></param>
        /// <returns></returns>
        public static ReferenceArray ToReferArray(this IEnumerable<Reference> references)
        {
            if (references is null)
                throw new ArgumentNullException(nameof(references));

            var results = new ReferenceArray();

            foreach (var refer in references)
                results.Append(refer);

            return results;
        }

        /// <summary>
        ///     Converts the Reference set to the ReferenceArray.
        /// </summary>
        /// <param name="references"></param>
        /// <returns></returns>
        public static ReferenceArray ToReferArray(params Reference[] references)
        {
            if (references is null)
                throw new ArgumentNullException(nameof(references));

            var results = new ReferenceArray();

            foreach (var refer in references)
                results.Append(refer);

            return results;
        }

        /// <summary>
        ///     Converts the ReferenceArray to the Reference list.
        /// </summary>
        /// <param name="references"></param>
        /// <returns></returns>
        public static List<Reference> ToReferArray(this ReferenceArray references)
        {
            if (references is null)
                throw new ArgumentNullException(nameof(references));

            var results = new List<Reference>();

            foreach (Reference refer in references)
                results.Add(refer);

            return results;
        }

        /// <summary>
        ///     Converts the CurveLoop list to the CurveArrArray.
        /// </summary>
        /// <param name="curveLoops"></param>
        /// <returns></returns>
        public static CurveArrArray ToCurveArrArray(this IEnumerable<CurveLoop> curveLoops)
        {
            if (curveLoops is null)
                throw new ArgumentNullException(nameof(curveLoops));

            var results = new CurveArrArray();

            foreach (var curveLoop in curveLoops)
                results.Append(curveLoop.ToCurveArray());

            return results;
        }

        /// <summary>
        ///     Converts the CurveLoop list to the CurveArrArray.
        /// </summary>
        /// <param name="curveLoops"></param>
        /// <returns></returns>
        public static CurveArrArray ToCurveArrArray(params CurveLoop[] curveLoops)
        {
            if (curveLoops is null)
                throw new ArgumentNullException(nameof(curveLoops));

            var results = new CurveArrArray();

            foreach (var curveLoop in curveLoops)
                results.Append(curveLoop.ToCurveArray());

            return results;
        }

        /// <summary>
        ///     Converts the CurveLoop list to the Curve list.
        /// </summary>
        /// <param name="curveLoops"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(this IEnumerable<CurveLoop> curveLoops)
        {
            if (curveLoops is null)
                throw new ArgumentNullException(nameof(curveLoops));

            return curveLoops.SelectMany(s => s).ToList();
        }

        /// <summary>
        ///     Converts the CurveLoop list to the Curve list.
        /// </summary>
        /// <param name="curveLoops"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(params CurveLoop[] curveLoops)
        {
            if (curveLoops is null)
                throw new ArgumentNullException(nameof(curveLoops));

            return curveLoops.SelectMany(s => s).ToList();
        }

        /// <summary>
        ///     Converts the CurveLoop list to the CurveArray list.
        /// </summary>
        /// <param name="curveLoops"></param>
        /// <returns></returns>
        public static List<CurveArray> ToCurveArrayList(this IEnumerable<CurveLoop> curveLoops)
        {
            if (curveLoops is null)
                throw new ArgumentNullException(nameof(curveLoops));

            return curveLoops.Select(s => s.ToCurveArray()).ToList();
        }

        /// <summary>
        ///     Converts the CurveLoop list to the CurveArray list.
        /// </summary>
        /// <param name="curveLoops"></param>
        /// <returns></returns>
        public static List<CurveArray> ToCurveArrayList(this CurveLoop[] curveLoops)
        {
            if (curveLoops is null)
                throw new ArgumentNullException(nameof(curveLoops));

            return curveLoops.Select(s => s.ToCurveArray()).ToList();
        }

        /// <summary>
        ///     Converts the CurveArrArray to the Curve list.
        /// </summary>
        /// <param name="curveArrArray"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(this CurveArrArray curveArrArray)
        {
            if (curveArrArray is null)
                throw new ArgumentNullException(nameof(curveArrArray));

            var results = new List<Curve>();

            foreach (CurveArray curves in curveArrArray)
                results.AddRange(curves.ToCurveList());

            return results;
        }

        /// <summary>
        ///     Converts the CurveArrArray to the CurveArray list.
        /// </summary>
        /// <param name="curveArrArray"></param>
        /// <returns></returns>
        public static List<CurveArray> ToCurveArrayList(this CurveArrArray curveArrArray)
        {
            if (curveArrArray is null)
                throw new ArgumentNullException(nameof(curveArrArray));

            var results = new List<CurveArray>();

            foreach (CurveArray curves in curveArrArray)
                results.Add(curves);

            return results;
        }

        /// <summary>
        ///     Converts the CurveArrArray to the CurveLoop list.
        /// </summary>
        /// <param name="curveArrArray"></param>
        /// <returns></returns>
        public static List<CurveLoop> ToCurveLoopList(this CurveArrArray curveArrArray)
        {
            if (curveArrArray is null)
                throw new ArgumentNullException(nameof(curveArrArray));

            var results = new List<CurveLoop>();

            foreach (CurveArray curves in curveArrArray)
                results.Add(curves.ToCurveLoop());

            return results;
        }

        /// <summary>
        ///     Converts the CurveArray list to the Curve list.
        /// </summary>
        /// <param name="curveArrays"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(this IEnumerable<CurveArray> curveArrays)
        {
            if (curveArrays is null)
                throw new ArgumentNullException(nameof(curveArrays));

            return curveArrays.SelectMany(s => s.ToCurveList()).ToList();
        }

        /// <summary>
        ///     Converts the CurveArray list to the Curve list.
        /// </summary>
        /// <param name="curveArrays"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(params CurveArray[] curveArrays)
        {
            if (curveArrays is null)
                throw new ArgumentNullException(nameof(curveArrays));

            return curveArrays.SelectMany(s => s.ToCurveList()).ToList();
        }

        /// <summary>
        ///     Converts the CurveArray list to the CurveArrArray.
        /// </summary>
        /// <param name="curveArrays"></param>
        /// <returns></returns>
        public static CurveArrArray ToCurveArrArray(this IEnumerable<CurveArray> curveArrays)
        {
            if (curveArrays is null)
                throw new ArgumentNullException(nameof(curveArrays));

            var results = new CurveArrArray();

            foreach (var curves in curveArrays)
                results.Append(curves);

            return results;
        }

        /// <summary>
        ///     Converts the CurveArray list to the CurveArrArray.
        /// </summary>
        /// <param name="curveArrays"></param>
        /// <returns></returns>
        public static CurveArrArray ToCurveArrArray(params CurveArray[] curveArrays)
        {
            if (curveArrays is null)
                throw new ArgumentNullException(nameof(curveArrays));

            var results = new CurveArrArray();

            foreach (var curves in curveArrays)
                results.Append(curves);

            return results;
        }

        /// <summary>
        ///     Converts the CurveArray list to the CurveLoop list.
        /// </summary>
        /// <param name="curveArrays"></param>
        /// <returns></returns>
        public static List<CurveLoop> ToCurveLoopList(this IEnumerable<CurveArray> curveArrays)
        {
            if (curveArrays is null)
                throw new ArgumentNullException(nameof(curveArrays));

            var results = new CurveArrArray();

            foreach (var curves in curveArrays)
                results.Append(curves);

            return results.ToCurveLoopList();
        }

        /// <summary>
        ///     Converts the CurveArray list to the CurveLoop list.
        /// </summary>
        /// <param name="curveArrays"></param>
        /// <returns></returns>
        public static List<CurveLoop> ToCurveLoopList(params CurveArray[] curveArrays)
        {
            if (curveArrays is null)
                throw new ArgumentNullException(nameof(curveArrays));

            var results = new CurveArrArray();

            foreach (var curves in curveArrays)
                results.Append(curves);

            return results.ToCurveLoopList();
        }

        /// <summary>
        ///     Converts the CurveLoop to the CurveArray.
        /// </summary>
        /// <param name="curveLoop"></param>
        /// <returns></returns>
        public static CurveArray ToCurveArray(this CurveLoop curveLoop)
        {
            if (curveLoop is null)
                throw new ArgumentNullException(nameof(curveLoop));

            var results = new CurveArray();

            foreach (var curve in curveLoop)
                results.Append(curve);

            return results;
        }

        /// <summary>
        ///     Converts the CurveLoop to the Curve list.
        /// </summary>
        /// <param name="curveLoop"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(this CurveLoop curveLoop)
        {
            if (curveLoop is null)
                throw new ArgumentNullException(nameof(curveLoop));

            var results = new List<Curve>();

            foreach (var curve in curveLoop)
                results.Add(curve);

            return results;
        }

        /// <summary>
        ///     Converts the CurveArray to the CurveLoop.
        /// </summary>
        /// <param name="curveArray"></param>
        /// <returns></returns>
        public static CurveLoop ToCurveLoop(this CurveArray curveArray)
        {
            if (curveArray is null)
                throw new ArgumentNullException(nameof(curveArray));

            var results = new CurveLoop();

            foreach (Curve curve in curveArray)
                results.Append(curve);

            return results;
        }

        /// <summary>
        ///     Converts the CurveArray to the Curve list.
        /// </summary>
        /// <param name="curveArray"></param>
        /// <returns></returns>
        public static List<Curve> ToCurveList(this CurveArray curveArray)
        {
            if (curveArray is null)
                throw new ArgumentNullException(nameof(curveArray));

            var results = new List<Curve>();

            foreach (Curve curve in curveArray)
                results.Add(curve);

            return results;
        }

        /// <summary>
        ///     Converts the Curve list to the CurveArray.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveArray ToCurveArray<T>(this IEnumerable<T> curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            var results = new CurveArray();

            foreach (var curve in curves)
                results.Append(curve);

            return results;
        }

        /// <summary>
        ///     Converts the Curve list to the CurveArray.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveArray ToCurveArray<T>(params T[] curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            var results = new CurveArray();

            foreach (var curve in curves)
                results.Append(curve);

            return results;
        }

        /// <summary>
        ///     Converts the Curve list to the CurveLoop.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveLoop ToCurveLoop<T>(this IEnumerable<T> curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            var results = new CurveLoop();

            foreach (var curve in curves)
                results.Append(curve);

            return results;
        }

        /// <summary>
        ///     Converts the Curve list to the CurveLoop.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveLoop ToCurveLoop<T>(params T[] curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            var results = new CurveLoop();

            foreach (var curve in curves)
                results.Append(curve);

            return results;
        }

        /// <summary>
        ///     Converts the Curve list to the CurveArrArray.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveArrArray ToCurveArrArray<T>(this IEnumerable<T> curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            var ary = new CurveArray();

            foreach (var curve in curves)
                ary.Append(curve);

            var results = new CurveArrArray();

            results.Append(ary);

            return results;
        }

        /// <summary>
        ///     Converts the Curve list to the CurveArrArray.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveArrArray ToCurveArrArray<T>(params T[] curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            var ary = new CurveArray();

            foreach (var curve in curves)
                ary.Append(curve);

            var results = new CurveArrArray();

            results.Append(ary);

            return results;
        }

        /// <summary>
        ///     Converts the Curve list to the CurveArray list.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static List<CurveArray> ToCurveArrayList<T>(this IEnumerable<T> curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            var ary = new CurveArray();

            foreach (var curve in curves)
                ary.Append(curve);

            return new List<CurveArray> { ary };
        }

        /// <summary>
        ///     Converts the Curve list to the CurveArray list.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static List<CurveArray> ToCurveArrayList<T>(params T[] curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            var ary = new CurveArray();

            foreach (var curve in curves)
                ary.Append(curve);

            return new List<CurveArray> { ary };
        }

        /// <summary>
        ///     Converts the Curve list to the CurveLoop list.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static List<CurveLoop> ToCurveLoopList<T>(this IEnumerable<T> curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            var loop = new CurveLoop();

            foreach (var curve in curves)
                loop.Append(curve);

            return new List<CurveLoop> { loop };
        }

        /// <summary>
        ///     Converts the Curve list to the CurveLoop list.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static List<CurveLoop> ToCurveLoopList<T>(params T[] curves) where T : Curve
        {
            if (curves is null)
                throw new ArgumentNullException(nameof(curves));

            var loop = new CurveLoop();

            foreach (var curve in curves)
                loop.Append(curve);

            return new List<CurveLoop> { loop };
        }

        /// <summary>
        ///     Converts the Face list to the FaceArray.
        /// </summary>
        /// <param name="faces"></param>
        /// <returns></returns>
        public static FaceArray ToFaceArray<T>(this IEnumerable<T> faces) where T : Face
        {
            if (faces is null)
                throw new ArgumentNullException(nameof(faces));

            var results = new FaceArray();

            foreach (var face in faces)
                results.Append(face);

            return results;
        }

        /// <summary>
        ///     Converts the Face list to the FaceArray.
        /// </summary>
        /// <param name="faces"></param>
        /// <returns></returns>
        public static FaceArray ToFaceArray<T>(params T[] faces) where T : Face
        {
            if (faces is null)
                throw new ArgumentNullException(nameof(faces));

            var results = new FaceArray();

            foreach (var face in faces)
                results.Append(face);

            return results;
        }

        /// <summary>
        ///     Converts the FaceArray to the Face list.
        /// </summary>
        /// <param name="faces"></param>
        /// <returns></returns>
        public static List<T> ToFaceList<T>(this FaceArray faces) where T : Face
        {
            if (faces is null)
                throw new ArgumentNullException(nameof(faces));

            var results = new List<T>();

            foreach (var face in faces)
            {
                if (face is T item)
                    results.Add(item);
            }

            return results;
        }

        /// <summary>
        ///     Gets the family symbol's raw location, it's for moving zero point.
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public static XYZ GetMinPoint(this CurveArrArray profile)
        {
            if (profile is null)
                throw new ArgumentNullException(nameof(profile));

            var curves = profile.Cast<CurveArray>().SelectMany(s => s.Cast<Curve>()).ToList();

            var pts = curves.GetDistinctPointList();

            pts = pts.OrderBy(o => o.Z).ThenBy(o => o.Y).ThenBy(o => o.X).ToList();

            return pts.FirstOrDefault();
        }
    }
}