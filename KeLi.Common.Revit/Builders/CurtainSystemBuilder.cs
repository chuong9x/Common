﻿/*
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
     |  |              Creation Time: 01/15/2020 01:21:11 PM |  |  |     |         |      |
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
using System.Linq;

using Autodesk.Revit.DB;

using KeLi.Common.Revit.Converters;
using KeLi.Common.Revit.Filters;
using KeLi.Common.Revit.Geometry;
using KeLi.Common.Revit.Relations;
using KeLi.Common.Revit.Widgets;

using static Autodesk.Revit.DB.BuiltInParameter;
using static Autodesk.Revit.DB.Structure.StructuralType;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    ///     CurtainSystem builder.
    /// </summary>
    public static class CurtainSystemBuilder
    {
        /// <summary>
        ///     Creates a new CurtainSystem for wall with transaction.
        /// </summary>
        /// <param name="wall"></param>
        /// <param name="room"></param>
        /// <param name="view3D"></param>
        /// <param name="typeName"></param>
        public static CurtainSystem CreateWallCurtainSystem(this Wall wall, SpatialElement room, View3D view3D, string typeName = null)
        {
            if (wall is null)
                throw new ArgumentNullException(nameof(wall));

            if (room is null)
                throw new ArgumentNullException(nameof(room));

            var face = wall.GetNearestPlanarFace(room, view3D);

            if (face == null)
                return null;

            var boundary = face.GetEdgesAsCurveLoops().ToCurveArrArray();

            var lvl = room.Level;

            return CreateCurtainSystem(boundary, lvl, face.FaceNormal, typeName);
        }

        /// <summary>
        ///     Creates a new CurtainSystem with transaction.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="lvl"></param>
        /// <param name="normal"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static CurtainSystem CreateCurtainSystem(CurveArrArray profile, Level lvl, XYZ normal, string typeName = null)
        {
            if (normal is null)
                throw new NullReferenceException(nameof(normal));

            var doc = lvl.Document;

            var location = profile.ToCurveList().GetDiffPointList().GetMinPoint();

            var plane = normal.CreatePlane(XYZ.Zero);

            var symbolParm = new ExtrudeParameter(profile, plane, 100);

            var symbol = doc.CreateExtrusion(symbolParm);

            return doc.AutoTransaction(() =>
            {
                var instance = doc.Create.NewFamilyInstance(location, symbol, lvl, NonStructural);

                doc.Regenerate();

                // The instance has thickness.
                var faces = instance.GetFaceList(-normal).ToFaceArray();

                var result = doc.CreateCurtainSystem(faces, typeName);

                doc.Delete(instance.Id);

                doc.Delete(symbol.Family.Id);

                var pnlTypeId = result.CurtainSystemType.get_Parameter(AUTO_PANEL).AsElementId();

                if (!(doc.GetElement(pnlTypeId) is PanelType pnlType))
                    return result;

                var thickness = pnlType.get_Parameter(CURTAIN_WALL_SYSPANEL_THICKNESS).AsDouble();

                ElementTransformUtils.MoveElement(doc, result.Id, normal * thickness / 2);

                return result;
            });
        }

        /// <summary>
        ///     Creates a new CurtainSystem.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="typeName"></param>
        /// <param name="faces"></param>
        /// <returns></returns>
        public static CurtainSystem CreateCurtainSystem(this Document doc, FaceArray faces, string typeName = null)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (typeName is null)
                throw new ArgumentNullException(nameof(typeName));

            if (faces is null)
                throw new ArgumentNullException(nameof(faces));

            var findType = doc.GetTypeElementList<CurtainSystemType>().FirstOrDefault(f => f.Name.Contains(typeName));

            if (findType != null)
                return doc.Create.NewCurtainSystem(faces, findType);

            var defaultTypeId = doc.GetDefaultElementTypeId(ElementTypeGroup.CurtainSystemType);

            var defaultType = doc.GetElement(defaultTypeId) as CurtainSystemType;

            var cloneType = defaultType?.Duplicate(typeName) as CurtainSystemType;

            return doc.Create.NewCurtainSystem(faces, cloneType);
        }
    }
}