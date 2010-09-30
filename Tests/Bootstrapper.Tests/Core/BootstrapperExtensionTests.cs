using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core
{
    [TestClass]
    public class BootstrapperExtensionTests
    {
        [TestMethod]
        public void ShouldCreateABoostrapperExtension()
        {
            //Act
            var result = new BootstrapperExtension();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BootstrapperExtension));

        }
    }
}
