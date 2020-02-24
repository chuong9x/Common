using System.IO;
using System.Linq;

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
            var stu1 = new TestC(1)  { Name = "Jack" };

            var stu2 = new TestC(2) { Name = "Tom" };

            JsonUtil.Serialize(new FileInfo("JsonTest.txt"), new[] { stu1,stu2 });
        }

        [TestMethod]
        public void DeserializeTest()
        {
            var stus = JsonUtil.Deserialize<TestC[]>(new FileInfo("JsonTest.txt"));

            Assert.AreEqual(stus.Count() == 2, true);
        }
    }
}