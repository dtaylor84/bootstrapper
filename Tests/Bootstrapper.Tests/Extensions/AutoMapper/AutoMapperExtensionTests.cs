using System.Collections.Generic;
using AutoMapper;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Bootstrap.Extensions.StartupTasks;
using Bootstrap.Tests.Extensions.TestImplementations;
using Bootstrap.Tests.Other;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.AutoMapper
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
        public void ShouldAddAutoMapperToExcludedAssemblies()
        {
            //Act
            var result = new AutoMapperExtension();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Bootstrapper.Excluding.Assemblies.Contains("AutoMapper"));
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

        [TestMethod]
        public void ShouldInvokeCreateMapForAllMapCreatorsWhenNoContainerExtensionHasBeenDeclared()
        {
            //Arrange
            var mapperExtension = new AutoMapperExtension();

            //Act
            mapperExtension.Run();
            
            //Assert
            Assert.IsNotNull(Mapper.Map<BootstrapperContainerExtension, IBootstrapperExtension>(new TestContainerExtension()));
            ExceptionAssert.Throws<AutoMapperMappingException>(() =>Mapper.Map<IBootstrapperExtension, BootstrapperContainerExtension>(new StartupTasksExtension()));
        }
    }
}
