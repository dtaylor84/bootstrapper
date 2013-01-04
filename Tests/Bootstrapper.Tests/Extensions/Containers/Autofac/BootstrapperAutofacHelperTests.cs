using Bootstrap.Autofac;
using Bootstrap.Extensions.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Containers.Autofac
{
    [TestClass]
    public class BootstrapperAutofacHelperTests
    {
        [TestMethod]
        public void ShouldAddTheAutofacExtensionToBootstrapper()
        {
            //Arrange
            Bootstrapper.ClearExtensions();

            //Act
            var result = Bootstrapper.With.Autofac();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[0], typeof(AutofacExtension));
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(BootstrapperContainerExtensionOptions));
        }
    }
}
