using AutoMapper;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions.Containers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.AutoMapper
{
    [TestClass]
    public class AutoMapperRegistrationTests
    {
        [TestMethod]
        public void ShouldCreateANewAutoMapperRegistration()
        {
            //Act
            var result = new AutoMapperRegistration();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperRegistration));
            Assert.IsInstanceOfType(result, typeof(Profile));
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
            A.CallTo(() => containerExtension.Register<IProfileExpression>(Mapper.Configuration)).MustHaveHappened();
            A.CallTo(() => containerExtension.Register(Mapper.Engine)).MustHaveHappened();
            A.CallTo(() => containerExtension.RegisterAll<IMapCreator>()).MustHaveHappened();
            A.CallTo(() => containerExtension.RegisterAll<Profile>()).MustHaveHappened();
        }

    }
}
