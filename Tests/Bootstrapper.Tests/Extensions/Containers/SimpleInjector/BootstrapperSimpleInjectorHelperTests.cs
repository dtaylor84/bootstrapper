using System;
using Bootstrap.Extensions.Containers;
using Bootstrap.SimpleInjector;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Containers.SimpleInjector
{
    [TestClass]
    public class BootstrapperSimpleInjectorHelperTests
    {
        [TestMethod]
        public void SimpleInjector_WhenInvoked_ShouldAddTheSimpleInjectorExtensionToBootstrapper()
        {
            //Arrange
            Bootstrapper.ClearExtensions();

            //Act
            var result = Bootstrapper.With.SimpleInjector();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[0], typeof(SimpleInjectorExtension));
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(BootstrapperContainerExtensionOptions));
        }
    }
}
