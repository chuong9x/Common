using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeLi.Common.Converter.Serializations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KeLi.Common.Converter.Serializations.Tests
{
    [TestClass()]
    public class JsonUtilTests
    {
        [TestMethod()]
        public void SerializeTest()
        {
            var stu1 = new TestC(1)  { Name = "Jack" };
            var stu2 = new TestC(2) { Name = "Tom" };

            JsonUtil.Serialize(new FileInfo("test.txt"), new TestC[] { stu1,stu2 });
        }

        [TestMethod()]
        public void DeserializeTest()
        {
            var stu = JsonUtil.Deserialize<TestC[]>(new FileInfo("test.txt"));

            //Assert.AreEqual(stu?.Name == "Jack", true);
        }
    }
}