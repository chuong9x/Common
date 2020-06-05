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

using static Autodesk.Revit.DB.ViewDetailLevel;

namespace KeLi.Common.Revit.Geometry
{
    /// <summary>
    ///     Mesh utility.
    /// </summary>
    public static class MeshUtil
    {
        /// <summary>
        ///     Gets the dispersed line list.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="gapNum"></param>
        /// <returns></returns>
        public static List<Line> GetDispersedLineList(this Curve curve, int gapNum = 0)
        {
            if (curve is null)
                throw new ArgumentNullException(nameof(curve));

            return curve.Tessellate().ToList().GetDispersedLineList(gapNum);
        }

        /// <summary>
        ///     Gets the element's triange list.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<Mesh, List<MeshTriangle>> GetMeshTrianglesDict(this Element elm)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            var results = new Dictionary<Mesh, List<MeshTriangle>>();

            var meshes = elm.GetMeshList();

            meshes.ForEach(f => results.Add(f, f.GetMeshTriangleList()));

            return results;
        }

        /// <summary>
        ///     Gets the element's triange list.
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public static List<MeshTriangle> GetMeshTriangleList(this Mesh mesh)
        {
            if (mesh is null)
                throw new ArgumentNullException(nameof(mesh));

            var results = new List<MeshTriangle>();

            for (var i = 0; i < mesh.NumTriangles; i++)
                results.Add(mesh.get_Triangle(i));

            return results;
        }

        /// <summary>
        ///     Gets the element's mesh list.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static List<Mesh> GetMeshList(this Element elm)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            var results = new List<Mesh>();

            elm.GetFaceList().ForEach(f => results.Add(f.Triangulate()));

            return results;
        }

        /// <summary>
        ///     Gets the element's face list.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static List<Face> GetFaceList(this Element elm)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            var results = new List<Face>();

            elm.GetValidSolidList().ForEach(f => results.AddRange(f.Faces.Cast<Face>()));

            return results;
        }

        /// <summary>
        ///     Gets the element's face set for the specified direction.
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static List<Face> GetFaceList(this Element elm, XYZ dir)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (dir is null)
                throw new ArgumentNullException(nameof(dir));

            var faces = elm.GetFaceList();

            var results = new List<Face>();

            foreach (var face in faces)
            {
                var box = face.GetBoundingBox();

                var min = box.Min;

                var noraml = face.ComputeNormal(min);

                if (noraml.AngleTo(dir) < 1e-6)
                    results.Add(face);
            }

            return results;
        }

        /// <summary>
        ///     Gets the element's face set for the specified direction list.
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="dirs"></param>
        /// <returns></returns>
        public static List<Face> GetFaceList(this Element elm, IEnumerable<XYZ> dirs)
        {
            if (dirs is null)
                throw new ArgumentNullException(nameof(dirs));

            var results = new List<Face>();

            foreach (var direction in dirs)
                results.AddRange(elm.GetFaceList(direction));

            return results;
        }

        /// <summary>
        ///     Gets the element's point set on face.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static List<XYZ> GetFacePointList(this Element elm)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            var results = new List<XYZ>();

            elm.GetEdgeList().ForEach(f => results.AddRange(f.Tessellate()));

            return results;
        }

        /// <summary>
        ///     Gets the element's point set base on distance.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static List<XYZ> GetSolidPointList(this Element elm)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            var pts = elm.GetFacePointList();

            var results = pts.OrderBy(o => o.X).ThenBy(o => o.Y).ThenBy(o => o.Z).ToList();

            for (var i = 0; i < results.Count; i++)
            {
                for (var j = i + 1; j < results.Count; j++)
                {
                    if (results[i] is null || results[j] is null)
                        continue;

                    if (results[j].GetRoundPoint(2).ToString() == results[i].GetRoundPoint(2).ToString())
                        results[j] = null;
                }
            }

            return results.Where(w => w != null).ToList();
        }

        /// <summary>
        ///     Gets the element's edge list.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static List<Edge> GetEdgeList(this Element elm)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            var results = new List<Edge>();

            elm.GetValidSolidList().ForEach(f => results.AddRange(f.Edges.Cast<Edge>()));

            return results;
        }

        /// <summary>
        ///     Gets the element's valid solid list.
        /// </summary>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static List<Solid> GetValidSolidList(this Element elm)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            var opt = new Options
            {
                ComputeReferences = true,
                DetailLevel = Coarse
            };

            var ge = elm.get_Geometry(opt);

            return ge is null ? new List<Solid>() : ge.GetValidSolidList();
        }

        /// <summary>
        ///     Gets the element's valid solid list.
        /// </summary>
        /// <param name="ge"></param>
        /// <returns></returns>
        public static List<Solid> GetValidSolidList(this GeometryElement ge)
        {
            if (ge is null)
                throw new ArgumentNullException(nameof(ge));

            var results = new List<Solid>();

            foreach (var obj in ge)
            {
                if (obj is Solid solid1 && solid1.Volume < 1e-9)
                    continue;

                if (obj is Solid solid)
                    results.Add(solid);

                else if (obj is GeometryInstance gi)
                {
                    var subGe = gi.GetInstanceGeometry();

                    if (subGe != null)
                        results = results.Union(subGe.GetValidSolidList()).ToList();

                    if (gi.GetSymbolGeometry() != null)
                        results = results.Union(subGe.GetValidSolidList()).ToList();
                }

                else if (obj is GeometryElement subGe)
                    results = results.Union(subGe.GetValidSolidList()).ToList();
            }

            return results;
        }

        /// <summary>
        ///     Gets the dispersed line list.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="gapNum"></param>
        /// <returns></returns>
        private static List<Line> GetDispersedLineList(this IEnumerable<XYZ> points, int gapNum = 0)
        {
            if (points is null)
                throw new ArgumentNullException(nameof(points));

            var tmpPoints = points.ToList();

            var results = new List<Line>();

            for (var i = 0; i < tmpPoints.Count - 1; i++)
            {
                var endIndex = i + gapNum + 1;

                if (endIndex >= tmpPoints.Count)
                    break;

                var line = Line.CreateBound(tmpPoints[i], tmpPoints[endIndex]);

                results.Add(line);

                i = endIndex - 1;
            }

            return results;
        }
    }
}