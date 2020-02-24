using KeLi.Common.Drive.Excel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeLi.Common.DriveTests.Excel
{
    [TestClass]
    public class ExcelParameterTests
    {
        [TestMethod]
        public void ExcelParamTest()
        {
            const string filePath = @"Resources\Instance.xlsx";

            const string tplPath = @"Resources\Template.xlsx";

            var param = new ExcelParameter(filePath, tplPath);

            Assert.AreEqual(param.FilePath.Length, param.TemplatePath.Length);
        }
    }
}