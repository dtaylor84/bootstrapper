using Bootstrap.Extensions.Containers;
using Bootstrap.Locator;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Locator
{
    [TestClass]
    public class ServiceLocatorConvenienceExtensionsTests
    {
        [TestInitialize]
        [TestCleanup]
        public void Initialize()
        {
            Bootstrapper.ClearExtensions();
        }

        [TestMethod]
        public void ShouldAddTheServiceLocatorExtensionToBootstrapper()
        {
            //Arrange
            Bootstrapper.ClearExtensions();
            var containerExtension = A.Fake<IBootstrapperContainerExtension>();

            //Act
            Bootstrapper.With.Extension(containerExtension).And.ServiceLocator();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[1], typeof(ServiceLocatorExtension));
        }
    }
}
