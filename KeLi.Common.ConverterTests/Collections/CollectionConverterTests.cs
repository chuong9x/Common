using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeLi.Common.Converter.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeLi.Common.Converter.Collections.Tests
{
    [TestClass()]
    public partial class CollectionConverterTests
    {
        [TestMethod()]
        public void ToAnyTypeTest()
        {
            var ta = new TestA(1);
            var result = ta.ToAnyType<TestA, TestB>();

            Assert.AreEqual(result != null, true);
        }
    }
}