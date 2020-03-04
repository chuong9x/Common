using KeLi.Common.Converter.Serializations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeLi.Common.ConverterTests.Serializations
{
    [TestClass]
    public class SoapUtilTests
    {
        [TestMethod]
        public void SerializeTest()
        {
            var stu = new Student(1) { Name = "Jack" };

            SoapUtil.Serialize("SoapTest.txt", stu);
        }

        [TestMethod]
        public void DeserializeTest()
        {
           var stus = SoapUtil.Deserialize<Student>("SoapTest.txt");

           Assert.AreEqual(stus.Name == "Jack", true);
        }
    }
}