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
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using KeLi.Common.Revit.Converters;
using KeLi.Common.Revit.Filters;
using KeLi.Common.Revit.Geometry;
using KeLi.Common.Revit.Relations;
using KeLi.Common.Revit.Widgets;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    /// CurtainSystem builder.
    /// </summary>
    public static class CurtainSystemBuilder
    {
        /// <summary>
        /// Creates a new CurtainSystem.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="faces"></param>
        /// <returns></returns>
        public static CurtainSystem CreateCurtainSystem(this Document doc, FaceArray faces)
        {
            var defaultTypeId = doc.GetDefaultElementTypeId(ElementTypeGroup.CurtainSystemType);
            var type = doc.GetElement(defaultTypeId) as CurtainSystemType;

            return doc.Create.NewCurtainSystem(faces, type);
        }

        /// <summary>
        /// Creates CurtainSystem list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="room"></param>
        /// <param name="app"></param>
        /// <param name="pnlType"></param>
        /// <param name="tplName"></param>
        public static void CreateCurtainSystemList(this Document doc, SpatialElement room, Application app, PanelType pnlType, string tplName)
        {
            var roomc = room.GetBoundingBox(doc).GetBoxCenter();
            var roomWalls = room.GetBoundaryWallList(doc);

            foreach (var roomWall in roomWalls)
            {
                var parm = new CurtainSystemParameter(roomWall, roomc, pnlType, tplName);

                doc.CreateCurtainSystem(app, parm);
            }
        }

        /// <summary>
        /// Creates CurtainSystem list.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        /// <param name="parms"></param>
        public static void CreateCurtainSystem(this Document doc, Application app, IEnumerable<CurtainSystemParameter> parms)
        {
            if (app == null)
                throw new NullReferenceException(nameof(app));

            if (parms == null)
                throw new NullReferenceException(nameof(parms));

            parms.ToList().ForEach(f => doc.CreateCurtainSystem(app, f));
        }

        /// <summary>
        /// Creates a new CurtainSystem. 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        /// <param name="parm"></param>
        public static void CreateCurtainSystem(this Document doc,  Application app, CurtainSystemParameter parm)
        {
            if (app == null)
                throw new NullReferenceException(nameof(app));

            if (parm == null)
                throw new NullReferenceException(nameof(parm));

            var wline = parm.RefWall.GetLocationCurve() as Line;
            var wdir = wline.GetLineDirection(parm.RefCenter);
            var innerNormal = wdir.GetInnerNormal();
            var face = parm.RefWall.GetInnerFace(parm.RefCenter);
            var profile = face.GetEdgesAsCurveLoops().ToCurveArrArray();
            var minPt = profile.ToCurveList().GetDistinctPointList().GetMinPoint();

#if R2016
            var plane = new Plane(innerNormal, XYZ.Zero);
#endif
#if !R2016
            var plane = Plane.CreateByNormalAndOrigin(innerNormal, XYZ.Zero);
#endif

            var symbolParm = new FamilySymbolParameter(parm.TemplateName, profile, plane, 1);
            var symbol = doc.CreateExtrusionSymbol(app, symbolParm);
            var lvl = doc.GetElement(parm.RefWall.LevelId) as Level;
            var instanceParm = new FamilyInstanceParameter(minPt, symbol, lvl, StructuralType.NonStructural);

            doc.AutoTransaction(() =>
            {
                var inst = doc.CreateFamilyInstance(instanceParm);

                doc.Regenerate();

                // The instance has thickness.
                var faces = inst.GetFaceList(-innerNormal).ToFaceArray();

                var curtain = doc.CreateCurtainSystem(faces);

                doc.Delete(inst.Id);
                doc.Delete(symbol.Family.Id);

                if (parm.PanelType != null)
                {
                    curtain.CurtainSystemType.get_Parameter(BuiltInParameter.AUTO_PANEL).Set(parm.PanelType.Id);

                    var thickness = parm.PanelType.get_Parameter(BuiltInParameter.CURTAIN_WALL_SYSPANEL_THICKNESS).AsDouble();

                    ElementTransformUtils.MoveElement(doc, curtain.Id, innerNormal * thickness / 2);
                }
            });
        }
    }
}
