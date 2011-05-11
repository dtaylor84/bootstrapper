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
            Bootstrapper.With.StructureMap();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[0], typeof(StructureMapExtension));
        }
    }
}
