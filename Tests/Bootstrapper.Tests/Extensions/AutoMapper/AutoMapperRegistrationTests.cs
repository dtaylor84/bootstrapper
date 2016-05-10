using AutoMapper;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions.Containers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.AutoMapper
{
    using global::AutoMapper.QueryableExtensions;

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
            A.CallTo(() => containerExtension.Register(AutoMapperExtension.ConfigurationProvider)).MustHaveHappened();
            A.CallTo(() => containerExtension.Register(AutoMapperExtension.ProfileExpression)).MustHaveHappened();
            A.CallTo(() => containerExtension.Register(AutoMapperExtension.Mapper)).MustHaveHappened();
            A.CallTo(() => containerExtension.Register(AutoMapperExtension.Engine)).MustHaveHappened();
            A.CallTo(() => containerExtension.Register<IExpressionBuilder, ExpressionBuilder>()).MustHaveHappened();
            A.CallTo(() => containerExtension.RegisterAll<IMapCreator>()).MustHaveHappened();
            A.CallTo(() => containerExtension.RegisterAll<Profile>()).MustHaveHappened();
        }

    }
}
