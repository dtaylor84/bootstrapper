using AutoMapper;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions.Containers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson.Serialization;

namespace Bootstrap.Tests.Extensions.MongoDB
{
    [TestClass]
    public class MongoRegistrationTests
    {
        [TestMethod]
        public void ShouldCreateANewAutoMapperRegistration()
        {
            //Act
            var result = new AutoMapperRegistration();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperRegistration));
            Assert.IsInstanceOfType(result, typeof(AutoMapperRegistration));
        }

        [TestMethod]
        public void ShouldInvokeRegisterAllForMapCreatorsAndProfilesInContainerExtension()
        {
            //Arrange
            var containerExtension = A.Fake<IBootstrapperContainerExtension>();

            //Act
            new AutoMapperRegistration().Register(containerExtension);

            //Assert
            A.CallTo(() => containerExtension.RegisterAll<BsonClassMap>()).MustHaveHappened();
        }

    }
}
