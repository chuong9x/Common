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
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

using KeLi.Common.Revit.Converters;
using KeLi.Common.Revit.Filters;
using KeLi.Common.Revit.Geometry;
using KeLi.Common.Revit.Relations;
using KeLi.Common.Revit.Widgets;

using static Autodesk.Revit.DB.BuiltInParameter;

using Room2 = Autodesk.Revit.DB.SpatialElement;
using App = Autodesk.Revit.ApplicationServices.Application;
using CurtainParm = KeLi.Common.Revit.Builders.CurtainSystemParameter;
using InstParm = KeLi.Common.Revit.Builders.FamilyInstanceParameter;
using SymbolParm = KeLi.Common.Revit.Builders.FamilySymbolParameter;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    ///     CurtainSystem builder.
    /// </summary>
    public static class CurtainSystemBuilder
    {
        /// <summary>
        ///     Creates CurtainSystem list for wall with transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        /// <param name="typeName"></param>
        /// <param name="tplName"></param>
        public static List<CurtainSystem> CreateCurtainWallList(this Document doc, App app, string typeName, string tplName)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (app is null)
                throw new ArgumentNullException(nameof(app));

            if (typeName is null)
                throw new NullReferenceException(nameof(typeName));

            if (tplName is null)
                throw new ArgumentNullException(nameof(tplName));

            var rooms = doc.GetSpatialElementList();

            var results = new List<CurtainSystem>();

            foreach (var room in rooms)
                results.AddRange(doc.CreateCurtainWallList(app, room, typeName, tplName));

            return results;
        }

        /// <summary>
        ///     Creates CurtainSystem list for wall with transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        /// <param name="room"></param>
        /// <param name="typeName"></param>
        /// <param name="tplName"></param>
        public static List<CurtainSystem> CreateCurtainWallList(this Document doc, App app, Room2 room, string typeName, string tplName)
        {
            if (doc is null)
                throw new NullReferenceException(nameof(doc));

            if (app is null)
                throw new NullReferenceException(nameof(app));

            if (room is null)
                throw new NullReferenceException(nameof(room));

            if (typeName is null)
                throw new NullReferenceException(nameof(typeName));

            if (tplName is null)
                throw new NullReferenceException(nameof(tplName));

            var walls = room.GetBoundaryWallList(doc);

            var view3D = doc.Get3DViewList().FirstOrDefault();

            var results = new List<CurtainSystem>();

            foreach (var wall in walls)
            {
                var parm = new CurtainParm(room, wall, typeName, tplName);

                results.Add(doc.CreateCurtainWall(app, parm, view3D));
            }

            return results;
        }

        /// <summary>
        ///     Creates CurtainSystem list for wall with transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        /// <param name="curtainParms"></param>
        public static List<CurtainSystem> CreateCurtainWallList(this Document doc, App app, IEnumerable<CurtainParm> curtainParms)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (app is null)
                throw new NullReferenceException(nameof(app));

            if (curtainParms is null)
                throw new NullReferenceException(nameof(curtainParms));

            var view3D = doc.Get3DViewList().FirstOrDefault();

            return curtainParms.Select(s => doc.CreateCurtainWall(app, s, view3D)).ToList();
        }

        /// <summary>
        ///     Creates a new CurtainSystem for wall with transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        /// <param name="curtainParm"></param>
        /// <param name="view3D"></param>
        public static CurtainSystem CreateCurtainWall(this Document doc, App app, CurtainParm curtainParm, View3D view3D)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (app is null)
                throw new NullReferenceException(nameof(app));

            if (curtainParm is null)
                throw new NullReferenceException(nameof(curtainParm));

            var face = curtainParm.ReferenceWall.GetNearestPlanarFace(curtainParm.Room, doc, view3D);

            if (face == null)
                return null;

            var profile = face.GetEdgesAsCurveLoops().ToCurveArrArray();

            return CreateCurtainSystem(doc, app, profile, curtainParm, face.FaceNormal);
        }

        /// <summary>
        ///     Creates CurtainSystem list for floor with transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        /// <param name="typeName"></param>
        /// <param name="tplName"></param>
        public static List<CurtainSystem> CreateCurtainFloorList(this Document doc, App app, string typeName, string tplName)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (app is null)
                throw new ArgumentNullException(nameof(app));

            if (tplName is null)
                throw new ArgumentNullException(nameof(tplName));

            var rooms = doc.GetSpatialElementList();

            var results = new List<CurtainSystem>();

            foreach (var room in rooms)
                results.Add(doc.CreateCurtainFloor(app, room, typeName, tplName));

            return results;
        }

        /// <summary>
        ///     Creates a CurtainSystem for floor with transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        /// <param name="room"></param>
        /// <param name="typeName"></param>
        /// <param name="tplName"></param>
        public static CurtainSystem CreateCurtainFloor(this Document doc, App app, Room2 room, string typeName, string tplName)
        {
            if (doc is null)
                throw new NullReferenceException(nameof(doc));

            if (room is null)
                throw new NullReferenceException(nameof(room));

            if (app is null)
                throw new NullReferenceException(nameof(app));

            if (tplName is null)
                throw new NullReferenceException(nameof(tplName));

            var parm = new CurtainParm(room, typeName, tplName);

            return doc.CreateCurtainFloor(app, parm);
        }

        /// <summary>
        ///     Creates CurtainSystem list for floor with transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        /// <param name="curtainParms"></param>
        public static List<CurtainSystem> CreateCurtainFloorList(this Document doc, App app, IEnumerable<CurtainParm> curtainParms)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (app is null)
                throw new NullReferenceException(nameof(app));

            if (curtainParms is null)
                throw new NullReferenceException(nameof(curtainParms));

            return curtainParms.Select(s => doc.CreateCurtainFloor(app, s)).ToList();
        }

        /// <summary>
        ///     Creates a new CurtainSystem for floor with transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        /// <param name="curtainParm"></param>
        /// <returns></returns>
        public static CurtainSystem CreateCurtainFloor(this Document doc, App app, CurtainParm curtainParm)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (app is null)
                throw new NullReferenceException(nameof(app));

            if (curtainParm is null)
                throw new NullReferenceException(nameof(curtainParm));

            var profile = curtainParm.Room.GetBoundaryLineList().ToCurveArrArray();

            return CreateCurtainSystem(doc, app, profile, curtainParm, XYZ.BasisZ);
        }

        /// <summary>
        ///     Creates CurtainSystem list for ceiling with transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        /// <param name="typeName"></param>
        /// <param name="tplName"></param>
        public static List<CurtainSystem> CreateCurtainCeilingList(this Document doc, App app, string typeName, string tplName)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (app is null)
                throw new ArgumentNullException(nameof(app));

            if (tplName is null)
                throw new ArgumentNullException(nameof(tplName));

            var rooms = doc.GetSpatialElementList();

            var results = new List<CurtainSystem>();

            foreach (var room in rooms)
                results.Add(doc.CreateCurtainCeiling(app, room, typeName, tplName));

            return results;
        }

        /// <summary>
        ///     Creates a CurtainSystem for ceiling with transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        /// <param name="room"></param>
        /// <param name="typeName"></param>
        /// <param name="tplName"></param>
        public static CurtainSystem CreateCurtainCeiling(this Document doc, App app, Room2 room, string typeName, string tplName)
        {
            if (doc is null)
                throw new NullReferenceException(nameof(doc));

            if (room is null)
                throw new NullReferenceException(nameof(room));

            if (app is null)
                throw new NullReferenceException(nameof(app));

            if (tplName is null)
                throw new NullReferenceException(nameof(tplName));

            var parm = new CurtainParm(room, typeName, tplName);

            return doc.CreateCurtainCeiling(app, parm);
        }

        /// <summary>
        ///     Creates CurtainSystem list for ceiling with transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        /// <param name="curtainParms"></param>
        public static List<CurtainSystem> CreateCurtainCeilingList(this Document doc, App app, IEnumerable<CurtainParm> curtainParms)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (app is null)
                throw new NullReferenceException(nameof(app));

            if (curtainParms is null)
                throw new NullReferenceException(nameof(curtainParms));

            return curtainParms.Select(s => doc.CreateCurtainCeiling(app, s)).ToList();
        }

        /// <summary>
        ///     Creates a new CurtainSystem for ceiling with transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        /// <param name="curtainParm"></param>
        /// <returns></returns>
        public static CurtainSystem CreateCurtainCeiling(this Document doc, App app, CurtainParm curtainParm)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (app is null)
                throw new NullReferenceException(nameof(app));

            if (curtainParm is null)
                throw new NullReferenceException(nameof(curtainParm));

            var profile = curtainParm.Room.GetBoundaryLineList().ToCurveArrArray();

            return CreateCurtainSystem(doc, app, profile, curtainParm, XYZ.BasisZ);
        }

        /// <summary>
        ///  Creates a new CurtainSystem with transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        /// <param name="profile"></param>
        /// <param name="curtainParm"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static CurtainSystem CreateCurtainSystem(this Document doc, App app, CurveArrArray profile, CurtainParm curtainParm, XYZ normal)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (app is null)
                throw new NullReferenceException(nameof(app));

            if (profile is null)
                throw new NullReferenceException(nameof(profile));

            if (curtainParm is null)
                throw new NullReferenceException(nameof(curtainParm));

            if (normal is null)
                throw new NullReferenceException(nameof(normal));

            var plane = normal.CreatePlane(XYZ.Zero);

            var symbolParm = new SymbolParm(curtainParm.TemplateFileName, profile, plane, 1.0);

            var symbol = doc.CreateExtrusionSymbol(app, symbolParm);

            var location = profile.ToCurveList().GetDistinctPointList().GetMinPoint();

            var instParm = new InstParm(location, symbol, curtainParm.Room.Level, StructuralType.NonStructural);

            return CreateCurtainSystem(doc, curtainParm, instParm, normal);
        }

        /// <summary>
        /// Creates a new CurtainSystem with transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="curtainParm"></param>
        /// <param name="instParm"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static CurtainSystem CreateCurtainSystem(this Document doc, CurtainParm curtainParm, InstParm instParm, XYZ normal)
        {
            if (doc is null)
                throw new ArgumentNullException(nameof(doc));

            if (curtainParm is null)
                throw new NullReferenceException(nameof(curtainParm));

            if (instParm is null)
                throw new NullReferenceException(nameof(instParm));

            if (normal is null)
                throw new NullReferenceException(nameof(normal));

            return doc.AutoTransaction(() =>
            {
                var inst = doc.CreateFamilyInstance(instParm);

                doc.Regenerate();

                // The instance has thickness.
                var faces = inst.GetFaceList(-normal).ToFaceArray();

                var result = doc.CreateCurtainSystem(curtainParm.TypeName, faces);

                doc.Delete(inst.Id);

                doc.Delete(instParm.Symbol.Family.Id);

                var pnlTypeId = result.CurtainSystemType.get_Parameter(AUTO_PANEL).AsElementId();

                if (!(doc.GetElement(pnlTypeId) is PanelType pnlType))
                    return result;

                var thickness = pnlType.get_Parameter(CURTAIN_WALL_SYSPANEL_THICKNESS).AsDouble();

                ElementTransformUtils.MoveElement(doc, result.Id, normal * thickness / 2);

                return result;
            });
        }

        /// <summary>
        ///     Creates a new CurtainSystem with transaction.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="typeName"></param>
        /// <param name="faces"></param>
        /// <returns></returns>
        public static CurtainSystem CreateCurtainSystem(this Document doc, string typeName, FaceArray faces)
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