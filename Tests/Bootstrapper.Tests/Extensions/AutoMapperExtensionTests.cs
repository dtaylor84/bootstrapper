using System.Collections.Generic;
using AutoMapper;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions
{
    [TestClass]
    public class AutoMapperExtensionTests
    {
        [TestMethod]
        public void ShouldCreateAnAutoMapperExtension()
        {
            //Act
            var result = new AutoMapperExtension();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperExtension));
            Assert.IsInstanceOfType(result, typeof(AutoMapperExtension));
        }

        [TestMethod]
        public void ShouldInvokeCreateMapForAllRegisteredMapCreators()
        {
            //Arrange
            var containerExtension = A.Fake<IBootstrapperContainerExtension>();
            var mapCreators = new List<IMapCreator> {A.Fake<IMapCreator>(), A.Fake<IMapCreator>()};
            var profileExpression = A.Fake<IProfileExpression>();
            A.CallTo(() => containerExtension.Resolve<IProfileExpression>()).Returns(profileExpression);
            A.CallTo(() => containerExtension.ResolveAll<IMapCreator>()).Returns(mapCreators);
            Bootstrapper.With.Extension(containerExtension);
            var mapperExtension = new AutoMapperExtension();

            //Act
            mapperExtension.Run();

            //Assert
            A.CallTo(() => containerExtension.Resolve<IProfileExpression>()).MustHaveHappened();
            A.CallTo(() => containerExtension.ResolveAll<IMapCreator>()).MustHaveHappened();
            foreach(var mapCreator in mapCreators)
            {
                var creator = mapCreator;
                A.CallTo(() => creator.CreateMap(profileExpression)).MustHaveHappened();
            }
        }

        [TestMethod]
        public void ShouldResetMapper()
        {
            //Arrange
            Mapper.CreateMap<object, object>();
            var mapperExtension = new AutoMapperExtension();
            Assert.AreNotEqual(0, Mapper.GetAllTypeMaps().Length);

            //Act
            mapperExtension.Reset();

            //Assert
            Assert.AreEqual(0, Mapper.GetAllTypeMaps().Length);
        }
    }
}
