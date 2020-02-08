using System.IO;
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
            var filePath = new FileInfo(@"Resources\Instance.xlsx");
            var tplPath = new FileInfo(@"Resources\Template.xlsx");
            var param = new ExcelParameter(filePath, tplPath);

            Assert.AreEqual(param.FilePath.Length, param.TemplatePath.Length);
        }
    }
}