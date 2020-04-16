using System;
using System.Linq;

using Autodesk.Revit.DB;

using KeLi.Common.Revit.Converters;
using KeLi.Common.Revit.Filters;

namespace KeLi.Common.Revit.Widgets
{
    /// <summary>
    ///     Profile utility.
    /// </summary>
    public static class ProfileUtil
    {
        /// <summary>
        ///     Gets sweep's profile.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="familyPath"></param>
        /// <returns></returns>
        public static CurveArrArray GetSweepProfile(Document doc, string familyPath)
        {
            if (doc == null)
                throw new NullReferenceException(nameof(doc));

            if (familyPath == null)
                throw new NullReferenceException(nameof(familyPath));

            var profileDoc = doc.Application.OpenDocumentFile(familyPath);

            var detailCurves = profileDoc.GetInstanceElementList<CurveElement>();

            var curves = detailCurves.Select(s => s.GeometryCurve);

            return curves.ToCurveArrArray();
        }

        /// <summary>
        ///     Gets sweep's profile.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        public static CurveArrArray GetSweepProfile2(Document doc, string symbolName)
        {
            if (doc == null)
                throw new NullReferenceException(nameof(doc));

            if (symbolName == null)
                throw new NullReferenceException(nameof(symbolName));

            var symbol = doc.GetTypeElementList<FamilySymbol>().FirstOrDefault(f => f.Name == symbolName);

            if(symbol == null)
                throw new NullReferenceException(nameof(symbol));

            var profileDoc = doc.EditFamily(symbol.Family);

            var detailCurves = profileDoc.GetInstanceElementList<CurveElement>();

            var curves = detailCurves.Select(s => s.GeometryCurve);

            return curves.ToCurveArrArray();
        }
    }
}