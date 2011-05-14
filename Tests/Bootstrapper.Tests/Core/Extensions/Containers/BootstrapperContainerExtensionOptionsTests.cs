using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.Containers
{
    [TestClass]
    public class BootstrapperContainerExtensionOptionsTests
    {
        [TestMethod]
        public void ShouldCreateABootstrapperContainerExtensionOptions()
        {
            //Act
            var result = new BootstrapperContainerExtensionOptions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(IBootstrapperExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(BootstrapperExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(BootstrapperContainerExtensionOptions));
        }

        [TestMethod]
        public void UseAutoRegistrationShouldBeFalseByDefault()
        {
            //Arrange
            var options = new BootstrapperContainerExtensionOptions();

            //Act
            var result = options.AutoRegistration;

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ShouldSetUseAutoRegistrationToTrue()
        {
            //Arrange
            var options = new BootstrapperContainerExtensionOptions();

            //Act
            var result = options.UsingAutoRegistration();

            //Assert
            Assert.IsTrue(options.AutoRegistration);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(BootstrapperContainerExtensionOptions));
            Assert.AreSame(options, result);
        }
    }
}
