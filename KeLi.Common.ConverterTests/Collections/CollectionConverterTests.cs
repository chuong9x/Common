using KeLi.Common.Converter.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeLi.Common.ConverterTests.Collections
{
    [TestClass]
    public partial class CollectionConverterTests
    {
        [TestMethod]
        public void ToAnyTypeTest()
        {
            var ta = new TestA(1);

            var result = ta.ToAnyType<TestA, TestB>();

            Assert.AreEqual(result != null, true);
        }
    }
}