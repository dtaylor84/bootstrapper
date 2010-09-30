using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Bootstrap.UnityExtension;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bootstrap.Tests.Extensions.Containers.Unity
{
    [TestClass]
    public class AutoMapperRegistrationTests
    {
        [TestMethod]
        public void ShouldRegisterMappingEngine()
        {
            //Arrange
            var container = new UnityContainer();

            //Act
            new AutoMapperRegistration().Register(container);
            var result = container.Resolve<IMappingEngine>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IMappingEngine));
            Assert.AreSame(Mapper.Engine, result);
        }

        [TestMethod]
        public void ShouldRegisterMapCreatorsFromAssembly()
        {
            //Arrange
            var container = new UnityContainer();
            var containerExtension = new Mock<IBootstrapperContainerExtension>();
            var collector = new Mock<IAssemblyCollector>();
            collector.Setup(c => c.Assemblies).Returns(new List<Assembly> { Assembly.GetExecutingAssembly() });
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
            var container = new UnityContainer();
            var containerExtension = new Mock<IBootstrapperContainerExtension>();
            var collector = new Mock<IAssemblyCollector>();
            collector.Setup(c => c.Assemblies).Returns(new List<Assembly>());
            collector.Setup(c => c.AssemblyNames).Returns(new List<string> { Assembly.GetExecutingAssembly().FullName });
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
