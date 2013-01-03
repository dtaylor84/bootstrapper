using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core
{
    [TestClass]
    public class UtilsTests
    {
        [TestMethod]
        public void Foreach_WhenInvokedOnAnIEnumerable_ShouldExecuteTheFunctionForEachMemberOfEnumerable()
        {
            //Arrange
            var items = new[] {"one", "two", "three"};

            //Act
            var result = string.Empty;
            items.ForEach(i => result += i);

            //Assert
            Assert.AreEqual("onetwothree", result);
            
        }
    }
}
