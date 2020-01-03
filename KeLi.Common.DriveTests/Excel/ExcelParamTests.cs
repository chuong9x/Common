using System.IO;
using KeLi.Common.Drive.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeLi.Common.DriveTests.Excel
{
    [TestClass()]
    public class ExcelParamTests
    {
        [TestMethod()]
        public void ExcelParamTest()
        {
            var filePath = new FileInfo(@"Resources\Instance.xlsx");
            var templatePath = new FileInfo(@"Resources\Template.xlsx");
            var param = new ExcelParam(filePath, templatePath);

            Assert.AreEqual(param.FilePath.Length, param.TemplatePath.Length);
        }
    }
}