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
     |  |              Creation Time: 05/06/2020 07:30:25 PM |  |  |     |         |      |
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

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;

namespace KeLi.Common.Revit.Builders
{
    /// <summary>
    ///     Material manager.
    /// </summary>
    public static class MaterialManager
    {
        /// <summary>
        ///     Sets the material's texture.
        /// </summary>
        /// <param name="material"></param>
        /// <param name="angle"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        public static void SetTexture(this Material material, double angle, double[] offset, double[] size)
        {
            var doc = material.Document;

            if (offset == null || offset.Length == 0)
                return;

            if (size == null || size.Length == 0)
                return;

            using (var editScope = new AppearanceAssetEditScope(doc))
            {
                var asset = editScope.Start(material.AppearanceAssetId);

                var angleProp = asset[UnifiedBitmap.TextureWAngle] as AssetPropertyDouble;
                angleProp.Value = angle;

                var xOffsetProp = asset[UnifiedBitmap.TextureRealWorldOffsetX] as AssetPropertyDistance;
                xOffsetProp.Value = UnitUtils.Convert(offset[0], DisplayUnitType.DUT_MILLIMETERS, xOffsetProp.DisplayUnitType);

                var yOffsetProp = asset[UnifiedBitmap.TextureRealWorldOffsetY] as AssetPropertyDistance;
                yOffsetProp.Value = UnitUtils.Convert(offset[1], DisplayUnitType.DUT_MILLIMETERS, yOffsetProp.DisplayUnitType);

                var xSizeProp = asset[UnifiedBitmap.TextureRealWorldScaleX] as AssetPropertyDistance;
                xSizeProp.Value = UnitUtils.Convert(size[0], DisplayUnitType.DUT_MILLIMETERS, xSizeProp.DisplayUnitType);

                var ySizeProp = asset[UnifiedBitmap.TextureRealWorldScaleY] as AssetPropertyDistance;
                ySizeProp.Value = UnitUtils.Convert(size[1], DisplayUnitType.DUT_MILLIMETERS, ySizeProp.DisplayUnitType);

                editScope.Commit(true);
            }
        }
    }
}