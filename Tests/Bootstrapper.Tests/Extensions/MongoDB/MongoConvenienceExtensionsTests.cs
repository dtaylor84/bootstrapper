using Bootstrap.AutoMapper;
using Bootstrap.Extensions.Containers;
using Bootstrapper.MongoDB;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.MongoDB
{
    
    [TestClass]
    public class MongoConvenienceExtensionsTests
    {
        [TestInitialize]
        [TestCleanup]
        public void Initialize()
        {
            Bootstrapper.ClearExtensions();
        }

        [TestMethod]
        public void ShouldAddTheMongoDbExtensionToBootstrapper()
        {
            //Arrange
            Bootstrapper.ClearExtensions();
            var containerExtension = A.Fake<IBootstrapperContainerExtension>();

            //Act
            Bootstrapper.With.Extension(containerExtension).And.MongoDB();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[1], typeof(MongoExtension));
        }
    }
}
