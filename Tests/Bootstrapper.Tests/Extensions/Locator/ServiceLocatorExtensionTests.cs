using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Bootstrap.Locator;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Locator
{
    [TestClass]
    public class ServiceLocatorExtensionTests
    {
        [TestMethod]
        public void ShouldCreateANewServiceLocatorExtension()
        {
            //Act
            var result = new ServiceLocatorExtension();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperExtension));
            Assert.IsInstanceOfType(result, typeof(ServiceLocatorExtension));
        }

        [TestMethod]
        public void ShouldInvokeSetServiceLocatorInTheContainerExtension()
        {
            //Arrange
            var containerExtension = A.Fake<IBootstrapperContainerExtension>();
            Bootstrapper.With.Extension(containerExtension);
            var locatorExtension = new ServiceLocatorExtension();

            //Act
            locatorExtension.Run();
            Bootstrapper.ClearExtensions();

            //Assert
            A.CallTo(() => containerExtension.SetServiceLocator()).MustHaveHappened();
        }

        [TestMethod]
        public void ShouldInvokeResetServiceLocatorInTheContainerExtension()
        {
            //Arrange
            var containerExtension = A.Fake<IBootstrapperContainerExtension>();
            Bootstrapper.With.Extension(containerExtension);
            var locatorExtension = new ServiceLocatorExtension();

            //Act
            locatorExtension.Reset();
            Bootstrapper.ClearExtensions();

            //Assert
            A.CallTo(() => containerExtension.ResetServiceLocator()).MustHaveHappened();
        }

    }
}
