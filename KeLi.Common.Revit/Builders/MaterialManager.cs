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

            using (AppearanceAssetEditScope editScope = new AppearanceAssetEditScope(doc))
            {
                Asset asset = editScope.Start(material.AppearanceAssetId);

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
