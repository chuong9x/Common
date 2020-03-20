using KeLi.Common.Converter.Serializations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeLi.Common.ConverterTests.Serializations
{
    [TestClass]
    public class JsonUtilTests
    {
        [TestMethod]
        public void SerializeTest()
        {
            var stu1 = new Student(1) { Name = "Jack" };

            var stu2 = new Student(2) { Name = "Tom" };

            JsonUtil.Serialize("JsonTest.txt", new[] { stu1, stu2 });
        }

        [TestMethod]
        public void DeserializeTest()
        {
            var stus = JsonUtil.Deserialize<Student[]>("JsonTest.txt");

            Assert.AreEqual(stus.Length == 2, true);
        }
    }
}