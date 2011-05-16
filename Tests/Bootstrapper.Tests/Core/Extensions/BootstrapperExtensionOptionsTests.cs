using Bootstrap.Extensions;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions
{
    [TestClass]
    public class BootstrapperExtensionOptionsTests
    {
        [TestMethod]
        public void ShouldCreateANewBootstrapperExtensionOptions()
        {
            //Act
            var result = new BootstrapperExtensionOptions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(BootstrapperExtensionOptions));
        }

        [TestMethod]
        public void ShouldReturnTheBootstrapperExtensions()
        {
            //Arrange
            Bootstrapper.ClearExtensions();
            var options = new BootstrapperExtensionOptions();

            //Act
            var result = options.And;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BootstrapperExtensions));
            Assert.AreSame(Bootstrapper.With, result);
        }

        [TestMethod]
        public void ShouldInvokeTheStartMethodOftheBootStrapper()
        {
            //Arrange
            var extension = A.Fake<IBootstrapperExtension>();
            Bootstrapper.With.Extension(extension);
            var options = new BootstrapperExtensionOptions();

            //Act
            options.Start();
            Bootstrapper.ClearExtensions();

            //Assert
            A.CallTo(() => extension.Run()).MustHaveHappened();
        }
    }
}
