using Bootstrap.Extensions.Containers;
using Bootstrap.StructureMap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Containers.StructureMap
{
    [TestClass]
    public class BootstrapperStructureMapHelperTests
    {
        [TestMethod]
        public void ShouldAddTheStructureMapExtensionToBootstrapper()
        {
            //Arrange
            Bootstrapper.ClearExtensions();

            //Act
            var result = Bootstrapper.With.StructureMap();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[0], typeof(StructureMapExtension));
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(BootstrapperContainerExtensionOptions));
        }
    }
}
