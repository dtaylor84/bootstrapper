using Bootstrap.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Containers.Windsor
{
    [TestClass]
    public class BootstrapperWindsorHelperTests
    {
        [TestMethod]
        public void ShouldAddTheWindsorExtensionToBootstrapper()
        {
            //Arrange
            Bootstrapper.ClearExtensions();

            //Act
            Bootstrapper.With.Windsor();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[0], typeof(WindsorExtension));
        }

    }
}
