using Bootstrap.AutoMapper;
using Bootstrap.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions
{
    [TestClass]
    public class AutoMapperExtensionTests
    {
        [TestMethod]
        public void ShouldCreateAnAutoMapperExtension()
        {
            //Act
            var result = new AutoMapperExtension();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperExtension));
            Assert.IsInstanceOfType(result, typeof(AutoMapperExtension));
        }
    }
}
