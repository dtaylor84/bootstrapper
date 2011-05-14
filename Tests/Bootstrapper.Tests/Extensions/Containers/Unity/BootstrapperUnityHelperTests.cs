using Bootstrap.Extensions.Containers;
using Bootstrap.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Containers.Unity
{
    [TestClass]
    public class BootstrapperUnityHelperTests
    {
        [TestMethod]
        public void ShouldAddTheUnityExtensionToBootstrapper()
        {
            //Arrange
            Bootstrapper.ClearExtensions();

            //Act
            var result = Bootstrapper.With.Unity();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[0], typeof(UnityExtension));
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(BootstrapperContainerExtensionOptions));
        }
    }
}
