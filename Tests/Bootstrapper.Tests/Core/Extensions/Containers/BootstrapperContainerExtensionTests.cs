using Bootstrap.Tests.Extensions.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.Containers
{
    [TestClass]
    public class BootstrapperContainerExtensionTests
    {

        [TestMethod]
        public void ShouldCreateATestContainerExtension()
        {
            //Act
            var result = new TestContainerExtension();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TestContainerExtension));
        }

        [TestMethod]
        public void ShouldSetTheBootstrapperContainer()
        {
            //Arrange
            var containerExtension = new TestContainerExtension();

            //Act
            Bootstrapper.With.Extension(containerExtension).Start();
            var result = Bootstrapper.Container;
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ShouldRegisterAndInvokeRegistrations()
        {
            //Arrange
            var containerExtension = new TestContainerExtension();

            //Act
            Bootstrapper.With.Extension(containerExtension).Start();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsTrue(containerExtension.RegistrationsRegistered);
            Assert.IsTrue(containerExtension.RegistrationsInvoked);
        }

        [TestMethod]
        public void ShouldResetTheBootstrapperContainer()
        {
            //Arrange
            var containerExtension = new TestContainerExtension();
            Bootstrapper.With.Extension(containerExtension).Start();

            //Act
            containerExtension.Reset();
            var result = Bootstrapper.Container;
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNull(result);
        }
    }
}
