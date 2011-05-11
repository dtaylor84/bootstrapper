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
            Bootstrapper.With.Unity();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[0], typeof(UnityExtension));
        }
    }
}
