using System.Collections.Generic;

using KeLi.Common.Drive.Excel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeLi.Common.DriveTests.Excel
{
    [TestClass]
    public class ExcelAssistTests
    {
        [TestMethod]
        public void As2DArrayTest()
        {

        }

        [TestMethod]
        public void AsCrossArrayTest()
        {

        }

        [TestMethod]
        public void AsDataTableTest()
        {
            var parm = new ExcelParameter(@"Resources\StudentList.xlsx");

            var data = parm.AsDataTable();

            Assert.AreEqual(data != null, true);
        }

        [TestMethod]
        public void AsListTest()
        {

        }

        [TestMethod]
        public void ToExcelTest()
        {
            var parm = new ExcelParameter(@"Resources\StudentList.xlsx", @"Resources\Template.xlsx");

            var students = new List<Student>
            {
                new Student("1", "Student1"),
                new Student("2", "Student2"),
                new Student("3", "Student3")
            };

            parm.ToExcel(students);
        }

        [TestMethod]
        public void SetExcelStyleTest()
        {

        }

        [TestMethod]
        public void GetExcelPackageTest()
        {

        }

        [TestMethod]
        public void GetDcrpTest()
        {

        }

        [TestMethod]
        public void GetSpanTest()
        {

        }

        [TestMethod]
        public void GetReferenceTest()
        {

        }

        [TestMethod]
        public void GetMegerValueTest()
        {

        }
    }
}