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
using Autodesk.Revit.UI.Selection;

using static System.Math;

namespace KeLi.Common.Revit.Geometry
{
    /// <summary>
    ///     Box utility.
    /// </summary>
    public static class BoxUtil
    {
        /// <summary>
        ///     Gets the bounding box.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static BoundingBoxXYZ GetBoundingBox(this Element elm)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            var doc = elm.Document;

            var box = elm.get_BoundingBox(doc.ActiveView);

            return new BoundingBoxXYZ
            {
                Min = box.Min,

                Max = box.Max
            };
        }

        /// <summary>
        ///     Gets the bounding box in plane.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static BoundingBoxXYZ GetPlaneBox(this Element elm)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            var box = elm.GetBoundingBox();

            return new BoundingBoxXYZ
            {
                Min = box.Min,

                Max = new XYZ(box.Max.X, box.Max.Y, box.Min.Z)
            };
        }

        /// <summary>
        ///     Gets the round box.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static BoundingBoxXYZ GetRoundBox(this BoundingBoxXYZ box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            return new BoundingBoxXYZ
            {
                Min = box.Min.GetRoundPoint(),

                Max = box.Max.GetRoundPoint()
            };
        }

        /// <summary>
        ///     Gets the intersection box between the box1 and the box2.
        /// </summary>
        /// <param name="box1"></param>
        /// <param name="box2"></param>
        /// <returns></returns>
        public static BoundingBoxXYZ GetCrossingBox(this BoundingBoxXYZ box1, BoundingBoxXYZ box2)
        {
            if (box1 is null)
                throw new ArgumentNullException(nameof(box1));

            if (box2 is null)
                throw new ArgumentNullException(nameof(box2));

            var box1Min = box1.Min;

            var box1Max = box1.Max;

            var box2Min = box2.Min;

            var box2Max = box2.Max;

            var minX = Max(box1Min.X, box2Min.X);

            var minY = Max(box1Min.Y, box2Min.Y);

            var minZ = Max(box1Min.Z, box2Min.Z);

            var maxX = Min(box1Max.X, box2Max.X);

            var maxY = Min(box1Max.Y, box2Max.Y);

            var maxZ = Min(box1Max.Z, box2Max.Z);

            return new BoundingBoxXYZ
            {
                Min = new XYZ(minX, minY, minZ),

                Max = new XYZ(maxX, maxY, maxZ)
            };
        }

        /// <summary>
        ///     Converts to bounding box.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static BoundingBoxXYZ ToBoundingBoxXYZ(this PickedBox box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            var minPt = box.Min;

            var maxPt = box.Max;

            var minX = Min(minPt.X, maxPt.X);

            var minY = Min(minPt.Y, maxPt.Y);

            var minZ = Min(minPt.Z, maxPt.Z);

            var maxX = Max(minPt.X, maxPt.X);

            var maxY = Max(minPt.Y, maxPt.Y);

            var maxZ = Max(minPt.Z, maxPt.Z);

            return new BoundingBoxXYZ
            {
                Min = new XYZ(minX, minY, minZ),

                Max = new XYZ(maxX, maxY, maxZ)
            };
        }

        /// <summary>
        ///     Gets the round box.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static BoundingBoxXYZ GetRoundBox(this Element elm)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            var box = elm.get_BoundingBox(elm.Document.ActiveView);

            return new BoundingBoxXYZ
            {
                Min = box.Min.GetRoundPoint(),

                Max = box.Max.GetRoundPoint()
            };
        }

        /// <summary>
        ///     Gets the plane edge set and z axis value equals no zero.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static List<Line> GetPlaneEdgeList(this BoundingBoxXYZ box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            var vectors = box.GetPlaneVectorList();

            var p1 = vectors[0];

            var p2 = vectors[1];

            var p3 = vectors[2];

            var p4 = vectors[3];

            var p12 = Line.CreateBound(p1, p2);

            var p23 = Line.CreateBound(p2, p3);

            var p34 = Line.CreateBound(p3, p4);

            var p41 = Line.CreateBound(p4, p1);

            return new List<Line> { p12, p23, p34, p41 };
        }

        /// <summary>
        ///     Gets the box's plane 4 vectors.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static List<XYZ> GetPlaneVectorList(this BoundingBoxXYZ box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            var p1 = box.Min;

            var p2 = new XYZ(box.Max.X, box.Min.Y, p1.Z);

            var p3 = new XYZ(box.Max.X, box.Max.Y, p1.Z);

            var p4 = new XYZ(p1.X, box.Max.Y, p1.Z);

            return new List<XYZ> { p1, p2, p3, p4 };
        }

        /// <summary>
        ///     Gets the space edge list.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static List<Line> GetSpaceEdgeList(this BoundingBoxXYZ box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            var vectors = box.GetSpaceVectorList();

            var p1 = vectors[0];

            var p2 = vectors[1];

            var p3 = vectors[2];

            var p4 = vectors[3];

            var p5 = vectors[4];

            var p6 = vectors[5];

            var p7 = vectors[6];

            var p8 = vectors[7];

            var p12 = Line.CreateBound(p1, p2);

            var p14 = Line.CreateBound(p1, p4);

            var p15 = Line.CreateBound(p1, p5);

            var p23 = Line.CreateBound(p2, p3);

            var p24 = Line.CreateBound(p2, p4);

            var p34 = Line.CreateBound(p3, p4);

            var p37 = Line.CreateBound(p3, p7);

            var p48 = Line.CreateBound(p4, p8);

            var p56 = Line.CreateBound(p5, p6);

            var p58 = Line.CreateBound(p5, p8);

            var p67 = Line.CreateBound(p6, p7);

            var p78 = Line.CreateBound(p7, p8);

            return new List<Line> { p12, p14, p15, p23, p24, p34, p37, p48, p56, p58, p67, p78 };
        }

        /// <summary>
        ///     Gets the box's space 8 vectors.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static List<XYZ> GetSpaceVectorList(this BoundingBoxXYZ box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            var p1 = box.Min;

            var p2 = new XYZ(box.Max.X, box.Min.Y, p1.Z);

            var p3 = new XYZ(box.Max.X, box.Max.Y, p1.Z);

            var p4 = new XYZ(p1.X, box.Max.Y, p1.Z);

            var p5 = new XYZ(p1.X, p1.Y, box.Max.Z);

            var p6 = new XYZ(box.Max.X, p1.Y, box.Max.Z);

            var p7 = new XYZ(p1.X, box.Max.Y, box.Max.Z);

            var p8 = box.Max;

            return new List<XYZ> { p1, p2, p3, p4, p5, p6, p7, p8 };
        }

        /// <summary>
        ///     Gets the box's center point.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static XYZ GetBoxCenter(this BoundingBoxXYZ box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            return (box.Max + box.Min) / 2;
        }

        /// <summary>
        ///     Gets the box's length on y axis dreiction.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static double GetBoxLength(this BoundingBoxXYZ box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            return box.Max.X - box.Min.X;
        }

        /// <summary>
        ///     Gets the box's width on x axis direction.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static double GetBoxWidth(this BoundingBoxXYZ box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            return box.Max.Y - box.Min.Y;
        }

        /// <summary>
        ///     Gets the box's height on z axis direction.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static double GetBoxHeight(this BoundingBoxXYZ box)
        {
            if (box is null)
                throw new ArgumentNullException(nameof(box));

            return box.Max.Z - box.Min.Z;
        }

        /// <summary>
        ///     Gets the round point with custom precision.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static XYZ GetRoundPoint(this XYZ point, int precision = 4)
        {
            if (point is null)
                throw new ArgumentNullException(nameof(point));

            var roundX = Round(point.X, precision);

            var roundY = Round(point.Y, precision);

            var roundZ = Round(point.Z, precision);

            return new XYZ(roundX, roundY, roundZ);
        }
    }
}