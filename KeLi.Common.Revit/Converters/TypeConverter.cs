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
using Autodesk.Revit.DB;

namespace KeLi.Common.Revit.Converters
{
    /// <summary>
    /// Type converter.
    /// </summary>
    public static class TypeConverter
    {
        /// <summary>
        /// Convers the space curve to the plane line.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Line ToPlaneLine(this Curve line)
        {
            if (line == null)
                throw new ArgumentNullException(nameof(line));

            var p1 = line.GetEndPoint(0).ToPlanePoint();
            var p2 = line.GetEndPoint(1).ToPlanePoint();

            return Line.CreateBound(p1, p2);
        }

        /// <summary>
        /// Convers the space point to the plane point.
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static XYZ ToPlanePoint(this XYZ pt)
        {
            if (pt == null)
                throw new ArgumentNullException(nameof(pt));

            return new XYZ(pt.X, pt.Y, 0);
        }

        /// <summary>
        /// Converts the Reference set to the ReferenceArray.
        /// </summary>
        /// <param name="refs"></param>
        /// <returns></returns>
        public static ReferenceArray ToReferArray(this IList<Reference> refs)
        {
            if (refs == null)
                throw new ArgumentNullException(nameof(refs));

            var results = new ReferenceArray();

            foreach (var refer in refs)
                results.Append(refer);

            return results;
        }

        /// <summary>
        /// Converts the reference set to the ReferenceArray.
        /// </summary>
        /// <param name="refs"></param>
        /// <returns></returns>
        public static ReferenceArray ToReferArray(this Reference[] refs)
        {
            if (refs == null)
                throw new ArgumentNullException(nameof(refs));

            var results = new ReferenceArray();

            foreach (var refer in refs)
                results.Append(refer);

            return results;
        }

        /// <summary>
        /// Converts the CurveArray set to the CurveArrArray.
        /// </summary>
        /// <param name="curvess"></param>
        /// <returns></returns>
        public static CurveArrArray ToCurveArrArray(this IList<CurveArray> curvess)
        {
            if (curvess == null)
                throw new ArgumentNullException(nameof(curvess));

            var results = new CurveArrArray();

            foreach (var curves in curvess)
                results.Append(curves);

            return results;
        }

        /// <summary>
        /// Converts the CurveLoop set to the CurveArrArray.
        /// </summary>
        /// <param name="curveLoops"></param>
        /// <returns></returns>
        public static CurveArrArray ToCurveArrArray(this IList<CurveLoop> curveLoops)
        {
            if (curveLoops == null)
                throw new ArgumentNullException(nameof(curveLoops));

            var results = new CurveArrArray();

            foreach (var curveLoop in curveLoops)
                results.Append(curveLoop.ToCurveArray());

            return results;
        }

        /// <summary>
        /// Converts the CurveArrArray to the CurveLoop list.
        /// </summary>
        /// <param name="curvess"></param>
        /// <returns></returns>
        public static List<CurveLoop> ToCurveLoopList(this CurveArrArray curvess)
        {
            if (curvess == null)
                throw new ArgumentNullException(nameof(curvess));

            var results = new List<CurveLoop>();

            foreach (CurveArray curves in curvess)
                results.Add(curves.ToCurveLoop());

            return results;
        }

        /// <summary>
        /// Converts the CurveLoop to the CurveArray.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveArray ToCurveArray(this CurveLoop curves)
        {
            if (curves == null)
                throw new ArgumentNullException(nameof(curves));

            var results = new CurveArray();

            foreach (Curve curve in curves)
                results.Append(curve);

            return results;
        }

        /// <summary>
        /// Converts the CurveArray to the CurveLoop.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveLoop ToCurveLoop(this CurveArray curves)
        {
            if (curves == null)
                throw new ArgumentNullException(nameof(curves));

            var results = new CurveLoop();

            foreach (Curve curve in curves)
                results.Append(curve);

            return results;
        }

        /// <summary>
        /// Converts the curve array set to the CurveArrArray.
        /// </summary>
        /// <param name="curvess"></param>
        /// <returns></returns>
        public static CurveArrArray ToCurveArrArray(this CurveArray[] curvess)
        {
            if (curvess == null)
                throw new ArgumentNullException(nameof(curvess));

            var results = new CurveArrArray();

            foreach (var curves in curvess)
                results.Append(curves);

            return results;
        }

        /// <summary>
        /// Converts the curve set to the CurveArray.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveArray ToCurveArray(this IList<Curve> curves)
        {
            if (curves == null)
                throw new ArgumentNullException(nameof(curves));

            var results = new CurveArray();

            foreach (var curve in curves)
                results.Append(curve);

            return results;
        }

        /// <summary>
        /// Converts the curve set to the CurveArray.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveArray ToCurveArray(this Curve[] curves)
        {
            if (curves == null)
                throw new ArgumentNullException(nameof(curves));

            var results = new CurveArray();

            foreach (var curve in curves)
                results.Append(curve);

            return results;
        }

        /// <summary>
        /// Converts the face set to the FaceArray.
        /// </summary>
        /// <param name="faces"></param>
        /// <returns></returns>
        public static FaceArray ToFaceArray(this IList<Face> faces)
        {
            if (faces == null)
                throw new ArgumentNullException(nameof(faces));

            var results = new FaceArray();

            foreach (var face in faces)
                results.Append(face);

            return results;
        }

        /// <summary>
        /// Converts the face set to the FaceArray.
        /// </summary>
        /// <param name="faces"></param>
        /// <returns></returns>
        public static FaceArray ToFaceArray(this Face[] faces)
        {
            if (faces == null)
                throw new ArgumentNullException(nameof(faces));

            var results = new FaceArray();

            foreach (var face in faces)
                results.Append(face);

            return results;
        }
    }
}