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
     |  |              Creation Time: 05/13/2020 06:02:02 PM |  |  |     |         |      |
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

using KeLi.Common.Revit.Filters;
using KeLi.Common.Revit.Properties;
using KeLi.Common.Revit.Widgets;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    ///     Wall sweep builder.
    /// </summary>
    public static class WallSweepBuilder
    {
        /// <summary>
        ///     Creates wall's sweep.
        /// </summary>
        /// <param name="wall"></param>
        /// <param name="sweepInfos"></param>
        public static void CreateWallSweep(this Wall wall, List<SweepInfo> sweepInfos)
        {
            var doc = wall.Document;
            var symbols = doc.GetTypeList<FamilySymbol>();
            var materials = doc.GetInstanceList<Material>();
            var sweepDatas = new List<SweepData>();

            var defIndex = sweepInfos.FindIndex(f => f.IsAbsolute);
            var defProfile = symbols.FirstOrDefault(f => f.Name == sweepInfos[defIndex].ProfileName);
            var defMaterial = materials.FirstOrDefault(f => f.Name == sweepInfos[defIndex].MaterialName);
            var defOffset = sweepInfos[defIndex].Distance;
            var defReverse = sweepInfos[defIndex].Flip;

            sweepDatas.Add(new SweepData(defIndex, defProfile.Id, defMaterial.Id, defOffset, defReverse));

            // ↑
            for (var i = defIndex - 1; i >= 0; i--)
            {
                var belowOffset = sweepInfos[i + 1].Distance;
                var belowProfile = symbols.FirstOrDefault(f => f.Name == sweepInfos[i + 1].ProfileName);

                if (belowProfile != null)
                {
                    var belowHeight = belowProfile.LookupParameter(Resources.WallSweep_Height).AsDouble();
                    var belowReverse = sweepInfos[i + 1].Flip;

                    var itemDistance = sweepInfos[i].Distance;
                    var itemProfile = symbols.FirstOrDefault(f => f.Name == sweepInfos[i].ProfileName);

                    if (itemProfile != null)
                    {
                        var itemHeight = itemProfile.LookupParameter(Resources.WallSweep_Height).AsDouble();
                        var itemReverse = sweepInfos[i].Flip;
                        var itemMaterialId = materials.FirstOrDefault(f => f.Name == sweepInfos[i].MaterialName)?.Id;

                        double itemOffset;

                        if (sweepInfos[i].IsAbsolute)
                            itemOffset = sweepInfos[i].Distance;

                        else if (!itemReverse && !belowReverse)
                            itemOffset = belowOffset + belowHeight + itemDistance;

                        else if (!itemReverse)
                            itemOffset = belowOffset + itemDistance;

                        else if (!belowReverse)
                            itemOffset = belowOffset + belowHeight + itemDistance + itemHeight;

                        else
                            itemOffset = belowOffset + itemDistance + itemHeight;

                        sweepInfos[i].Distance = itemOffset;
                        sweepDatas.Add(new SweepData(i, itemProfile.Id, itemMaterialId, itemOffset, itemReverse));
                    }
                }
            }

            // ↓
            for (var i = defIndex + 1; i < sweepInfos.Count; i++)
            {
                var aboveOffset = sweepInfos[i - 1].Distance;
                var aboveProfile = symbols.FirstOrDefault(f => f.Name == sweepInfos[i - 1].ProfileName);

                if (aboveProfile != null)
                {
                    var aboveHeight = aboveProfile.LookupParameter(Resources.WallSweep_Height).AsDouble();
                    var aboveReverse = sweepInfos[i - 1].Flip;

                    var itemDistance = sweepInfos[i].Distance;
                    var itemProfile = symbols.FirstOrDefault(f => f.Name == sweepInfos[i].ProfileName);

                    if (itemProfile != null)
                    {
                        var itemHeight = itemProfile.LookupParameter(Resources.WallSweep_Height).AsDouble();
                        var itemReverse = sweepInfos[i].Flip;
                        var itemMaterialId = materials.FirstOrDefault(f => f.Name == sweepInfos[i].MaterialName)?.Id;
                        double itemOffset;

                        if (sweepInfos[i].IsAbsolute)
                            itemOffset = sweepInfos[i].Distance;

                        else if (!itemReverse && !aboveReverse)
                            itemOffset = aboveOffset - itemDistance - itemHeight;

                        else if (!itemReverse)
                            itemOffset = aboveOffset - aboveHeight - itemDistance - itemHeight;

                        else if (!aboveReverse)
                            itemOffset = aboveOffset - itemDistance;

                        else
                            itemOffset = aboveOffset - aboveHeight - itemDistance;

                        sweepInfos[i].Distance = itemOffset;
                        sweepDatas.Add(new SweepData(i, itemProfile.Id, itemMaterialId, itemOffset, itemReverse, sweepInfos[i].IsAbsolute));
                    }
                }
            }

            doc.AutoTransaction(() =>
            {
                var wallType = wall.WallType.Duplicate(Guid.NewGuid().ToString()) as WallType;

                wall.WallType = wallType;

                if (wallType != null)
                {
                    var cs = wallType.GetCompoundStructure();

                    var sweeps = cs.GetWallSweepsInfo(WallSweepType.Sweep);

                    foreach (var item in sweeps)
                        cs.RemoveWallSweep(WallSweepType.Sweep, item.Id);

                    wallType.SetCompoundStructure(cs);
                }
            });

            sweepDatas = sweepDatas.OrderBy(o => o.Index).ToList();

            for (var i = 0; i < sweepDatas.Count; i++)
            {
                var materialId = sweepDatas[i].MaterialId;
                var profileId = sweepDatas[i].ProfileId;
                var offset = sweepDatas[i].Offset;
                var flip = sweepDatas[i].Flip;

                CreateWallSweep(wall.WallType, i, materialId, profileId, offset, flip);
            }
        }

        /// <summary>
        ///     Creates wall's sweep.
        /// </summary>
        /// <param name="wallType"></param>
        /// <param name="index"></param>
        /// <param name="materialId"></param>
        /// <param name="profileId"></param>
        /// <param name="offset"></param>
        /// <param name="reverse"></param>
        public static void CreateWallSweep(WallType wallType, int index, ElementId materialId, ElementId profileId, double offset, bool reverse)
        {
            var doc = wallType.Document;

            doc.AutoTransaction(() =>
            {
                var cs = wallType.GetCompoundStructure();

                var sweepInfo = new WallSweepInfo(true, WallSweepType.Sweep)
                {
                    Id = index + 1
                };

                if (materialId != null)
                    sweepInfo.MaterialId = materialId;

                if (profileId != null)
                    sweepInfo.ProfileId = profileId;

                sweepInfo.Distance = offset;
                sweepInfo.IsProfileFlipped = reverse;

                cs.AddWallSweep(sweepInfo);
                wallType.SetCompoundStructure(cs);
            });
        }

        /// <summary>
        ///     Wall sweep data.
        /// </summary>
        public class SweepData
        {
            /// <summary>
            ///     Wall sweep data.
            /// </summary>
            /// <param name="index"></param>
            /// <param name="profileId"></param>
            /// <param name="materialId"></param>
            /// <param name="offset"></param>
            /// <param name="flip"></param>
            /// <param name="isAbsolute"></param>
            public SweepData(int index, ElementId profileId, ElementId materialId, double offset, bool flip, bool isAbsolute = false)
            {
                Index = index;
                ProfileId = profileId;
                MaterialId = materialId;
                Offset = offset;
                Flip = flip;
                IsAbsolute = isAbsolute;
            }

            /// <summary>
            ///     Index
            /// </summary>
            public int Index { get; set; }

            /// <summary>
            ///     ProfileId
            /// </summary>
            public ElementId ProfileId { get; set; }

            /// <summary>
            ///     MaterialId
            /// </summary>
            public ElementId MaterialId { get; set; }

            /// <summary>
            ///     Offset
            /// </summary>
            public double Offset { get; set; }

            /// <summary>
            ///     Flip
            /// </summary>
            public bool Flip { get; set; }

            /// <summary>
            ///     IsAbsolute
            /// </summary>
            public bool IsAbsolute { get; set; }
        }
    }
}