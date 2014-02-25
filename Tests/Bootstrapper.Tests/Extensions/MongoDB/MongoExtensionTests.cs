using System.Collections.Generic;
using Bootstrapper.MongoDB;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson.Serialization;

namespace Bootstrap.Tests.Extensions.MongoDB
{
    [TestClass]
    public class MongoExtensionTests
    {
        private IRegistrationHelper registrationHelper;

        [TestInitialize]
        public void Initialize()
        {
            Bootstrapper.ClearExtensions();
            registrationHelper = A.Fake<IRegistrationHelper>();
        }

        [TestMethod]
        public void ShouldCreateAMongoExtension()
        {
            //Act
            var result = new MongoExtension(registrationHelper);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperExtension));
            Assert.IsInstanceOfType(result, typeof(MongoExtension));
        }

        [TestMethod]
        public void ShouldAddMongoToExcludedAssemblies()
        {
            //Act
            var result = new MongoExtension(registrationHelper);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Bootstrapper.Excluding.Assemblies.Contains("MongoDB.Bson"));
            Assert.IsTrue(Bootstrapper.Excluding.Assemblies.Contains("MongoDB.Driver"));
        }

        [TestMethod]
        public void ShouldInvokeConfigureForAllRegisteredProfiles()
        {
            //Arrange
            var containerExtension = A.Fake<IBootstrapperContainerExtension>();
            var profiles = new List<BsonClassMap> { new TestMongoClassMap() };
            A.CallTo(() => containerExtension.ResolveAll<BsonClassMap>()).Returns(profiles);
            Bootstrapper.With.Extension(containerExtension);
            var mapperExtension = new MongoExtension(registrationHelper);

            //Act
            mapperExtension.Run();

            //Assert
            A.CallTo(() => containerExtension.ResolveAll<BsonClassMap>()).MustHaveHappened();
            Assert.IsTrue(BsonClassMap.IsClassMapRegistered(typeof(TestMongo)));
        }

        [TestMethod]
        public void ShouldInvokeConfigureForAllProfilesWhenNoContainerExtensionHasBeenDeclared()
        {
            //Arrange
            var profiles = new List<BsonClassMap> { new TestMongoClassMap() };
            A.CallTo(() => registrationHelper.GetInstancesOfTypesImplementing<BsonClassMap>()).Returns(profiles);
            var mapperExtension = new MongoExtension(registrationHelper);

            //Act
            mapperExtension.Run();

            //Assert
            A.CallTo(() => registrationHelper.GetInstancesOfTypesImplementing<BsonClassMap>()).MustHaveHappened();
            Assert.IsTrue(BsonClassMap.IsClassMapRegistered(typeof(TestMongo)));
        }
    }
}
