using System.Linq;
using Bootstrap.Extensions.Containers;
using Bootstrap.StructureMap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Containers.StructureMap
{
    [TestClass]
    public class StructureMapConvenienceExtensionsTests
    {
        [TestInitialize]
        [TestCleanup]
        public void InitializeBootstrapper()
        {
            Bootstrapper.ClearExtensions();
        }

        [TestMethod]
        public void ShouldAddTheStructureMapExtensionToBootstrapper()
        {
            //Act
            var result = Bootstrapper.With.StructureMap();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[0], typeof(StructureMapExtension));
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(StructureMapOptions));
        }

        [TestMethod]
        public void StructureMap_WhenInvoked_ShouldPassTheBootstrapperRegistrationHelperToTheConstructorOfTheExtension()
        {
            //Act
            Bootstrapper.With.StructureMap();

            //Assert
            var extension = Bootstrapper.GetExtensions().First() as StructureMapExtension;
            Assert.IsNotNull(extension);
            Assert.AreSame(Bootstrapper.RegistrationHelper, extension.Registrator);
        }
    }
}
