using System.Collections.Generic;
using AutoMapper;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Bootstrap.Tests.Extensions.TestImplementations;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.AutoMapper
{
    [TestClass]
    public class AutoMapperExtensionTests
    {
        private IRegistrationHelper registrationHelper;

        [TestInitialize]
        public void Initialize()
        {
            Bootstrapper.ClearExtensions();
            registrationHelper = A.Fake<IRegistrationHelper>();
        }

        [TestMethod]
        public void ShouldCreateAnAutoMapperExtension()
        {
            //Act
            var result = new AutoMapperExtension(registrationHelper);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperExtension));
            Assert.IsInstanceOfType(result, typeof(AutoMapperExtension));
        }

        [TestMethod]
        public void ShouldAddAutoMapperToExcludedAssemblies()
        {
            //Act
            var result = new AutoMapperExtension(registrationHelper);

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
            var mapperExtension = new AutoMapperExtension(registrationHelper);

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
        public void ShouldInvokeConfigureForAllRegisteredProfiles()
        {
            //Arrange
            Mapper.Reset();
            var containerExtension = A.Fake<IBootstrapperContainerExtension>();
            var profiles = new List<Profile> { new TestAutoMapperProfile() };
            A.CallTo(() => containerExtension.Resolve<IProfileExpression>()).Returns(Mapper.Configuration);
            A.CallTo(() => containerExtension.ResolveAll<Profile>()).Returns(profiles);
            Bootstrapper.With.Extension(containerExtension);
            var mapperExtension = new AutoMapperExtension(registrationHelper);
            var from = A.Fake<ITestInterface>();

            //Act
            mapperExtension.Run();
            var result = Mapper.Map<ITestInterface, TestImplementation>(from);

            //Assert
            A.CallTo(() => containerExtension.Resolve<IProfileExpression>()).MustHaveHappened();
            A.CallTo(() => containerExtension.ResolveAll<Profile>()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TestImplementation));
        }


        [TestMethod]
        public void ShouldResetMapper()
        {
            //Arrange
            Mapper.CreateMap<object, object>();
            var mapperExtension = new AutoMapperExtension(registrationHelper);
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
            var mapCreators = new List<IMapCreator> { A.Fake<IMapCreator>(), A.Fake<IMapCreator>() };
            A.CallTo(() => registrationHelper.GetInstancesOfTypesImplementing<IMapCreator>()).Returns(mapCreators);
            var mapperExtension = new AutoMapperExtension(registrationHelper);

            //Act
            mapperExtension.Run();
            
            //Assert
            A.CallTo(() => registrationHelper.GetInstancesOfTypesImplementing<IMapCreator>()).MustHaveHappened();
            mapCreators.ForEach(m => A.CallTo(() => m.CreateMap(A<IProfileExpression>.Ignored)).MustHaveHappened());
        }

        [TestMethod]
        public void ShouldInvokeConfigureForAllProfilesWhenNoContainerExtensionHasBeenDeclared()
        {
            //Arrange
            var profiles = new List<Profile> { new TestAutoMapperProfile() };
            A.CallTo(() => registrationHelper.GetInstancesOfTypesImplementing<Profile>()).Returns(profiles);
            var mapperExtension = new AutoMapperExtension(registrationHelper);
            var from = A.Fake<ITestInterface>();

            //Act
            mapperExtension.Run();
            var result = Mapper.Map<ITestInterface, TestImplementation>(from);

            //Assert
            A.CallTo(() => registrationHelper.GetInstancesOfTypesImplementing<Profile>()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TestImplementation));
        }

    }
}
