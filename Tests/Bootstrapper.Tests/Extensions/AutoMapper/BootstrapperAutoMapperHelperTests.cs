using Bootstrap.AutoMapper;
using Bootstrap.Extensions.Containers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.AutoMapper
{
    
    [TestClass]
    public class BootstrapperAutoMapperHelperTests
    {
        [TestInitialize]
        [TestCleanup]
        public void Initialize()
        {
            Bootstrapper.ClearExtensions();
        }

        [TestMethod]
        public void ShouldAddTheAutoMapperExtensionToBootstrapper()
        {
            //Arrange
            Bootstrapper.ClearExtensions();
            var containerExtension = A.Fake<IBootstrapperContainerExtension>();

            //Act
            Bootstrapper.With.Extension(containerExtension).And.AutoMapper();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[1], typeof(AutoMapperExtension));
        }
    }
}
