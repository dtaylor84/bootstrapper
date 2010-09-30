using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Bootstrap.WindsorExtension;
using Castle.Facilities.FactorySupport;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace Bootstrap.Tests.Extensions.Containers.Windsor
{
    [TestClass]
    public class AutoMapperRegistrationTests
    {
        [TestMethod]
        public void ShouldRegisterMappingEngine()
        {
            //Arrange
            var container = new WindsorContainer().AddFacility<FactorySupportFacility>(); 

            //Act
            new AutoMapperRegistration().Register(container);
            var result = container.Resolve<IMappingEngine>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(IMappingEngine));
            Assert.AreSame(Mapper.Engine, result);
        }

        [TestMethod]
        public void ShouldRegisterMapCreatorsFromAssembly()
        {
            //Arrange
            var container = new WindsorContainer().AddFacility<FactorySupportFacility>(); 
            var containerExtension = new Mock<IBootstrapperContainerExtension>();
            var collector = new Mock<IAssemblyCollector>();
            collector.Setup(c => c.Assemblies).Returns(new List<Assembly> {Assembly.GetExecutingAssembly()});
            collector.Setup(c => c.AssemblyNames).Returns(new List<string>());
            containerExtension.Setup(c => c.LookForMaps).Returns(collector.Object);
            Bootstrap.Bootstrapper.With.Container(containerExtension.Object);

            //Act
            new AutoMapperRegistration().Register(container);
            var result = container.ResolveAll<IMapCreator>();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IMapCreator>));
            Assert.IsTrue(result.Any(m => m.GetType() == typeof(BootStrapperExtensionMapCreator)));
        }

        [TestMethod]
        public void ShouldRegisterMapCreatorsFromAssemblyName()
        {
            //Arrange
            var container = new WindsorContainer().AddFacility<FactorySupportFacility>();
            var containerExtension = new Mock<IBootstrapperContainerExtension>();
            var collector = new Mock<IAssemblyCollector>();
            collector.Setup(c => c.Assemblies).Returns(new List<Assembly>());
            collector.Setup(c => c.AssemblyNames).Returns(new List<string> {Assembly.GetExecutingAssembly().FullName});
            containerExtension.Setup(c => c.LookForMaps).Returns(collector.Object);
            Bootstrap.Bootstrapper.With.Container(containerExtension.Object);

            //Act
            new AutoMapperRegistration().Register(container);
            var result = container.ResolveAll<IMapCreator>();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IMapCreator>));
            Assert.IsTrue(result.Any(m => m.GetType() == typeof(BootStrapperExtensionMapCreator)));
        }

    }
}
