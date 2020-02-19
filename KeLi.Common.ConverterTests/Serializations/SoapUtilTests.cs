using System.Collections.Generic;
using System.IO;
using KeLi.Common.Converter.Serializations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeLi.Common.ConverterTests.Serializations
{
    [TestClass()]
    public class SoapUtilTests
    {
        [TestMethod()]
        public void SerializeTest()
        {
            var test = new TestC(1) { Name = "Jack" };

            SoapUtil.Serialize(new FileInfo("SoapTest.txt"), test);
        }

        [TestMethod()]
        public void DeserializeTest()
        {
           var test = SoapUtil.Deserialize<TestC>(new FileInfo("SoapTest.txt"));

           Assert.AreEqual(test.Name == "Jack", true);
        }
    }
}