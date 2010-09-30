using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bootstrap.StructureMapExtension;
using Microsoft.Practices.ServiceLocation;
using StructureMap;
using StructureMap.ServiceLocatorAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Containers.StructureMap
{
    [TestClass]
    public class StructureMapContainerExtensionTests
    {
        [TestMethod]
        public void ShouldCreateAStructureMapContainerExtension()
        {
            //Act
            var result = new StructureMapContainerExtension();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StructureMapContainerExtension));
        }

        [TestMethod]
        public void ShouldReturnANullContainer()
        {
            //Arrange
            var containerExtension = new StructureMapContainerExtension();

            //Act
            var result = containerExtension.Container;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldReturnAnIContainer()
        {
            //Arrange
            var containerExtension = new StructureMapContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container;
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IContainer));
        }

        [TestMethod]
        public void ShouldRegisterAutoMapperRegistration()
        {
            //Arrange
            var containerExtension = new StructureMapContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container.GetAllInstances<IStructureMapRegistration>();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IStructureMapRegistration>));
            Assert.IsTrue(result.Any(t => t.GetType() == typeof(AutoMapperRegistration)));
        }

        [TestMethod]
        public void ShouldRegisterStartupTaskRegistration()
        {
            //Arrange
            var containerExtension = new StructureMapContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container.GetAllInstances<IStructureMapRegistration>();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IStructureMapRegistration>));
            Assert.IsTrue(result.Any(t => t.GetType() == typeof(StartupTaskRegistration)));
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfIStructureMapRegistration()
        {
            //Arrange
            var containerExtension = new StructureMapContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container.GetAllInstances<IStructureMapRegistration>();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IStructureMapRegistration>));
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIStructureMapRegistrationTypes()
        {
            //Arrange
            var containerExtension = new StructureMapContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.LookForRegistrations.InAssembly(Assembly.GetAssembly(typeof(TestStructureMapRegistration)));
            containerExtension.Run();
            var result = containerExtension.Container.GetInstance<StructureMapContainerExtension>();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StructureMapContainerExtension));
        }

        [TestMethod]
        public void ShouldSetTheServiceLocator()
        {
            //Arrange
            ServiceLocator.SetLocatorProvider(() => null);
            var containerExtension = new StructureMapContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = ServiceLocator.Current;
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StructureMapServiceLocator));
        }

        [TestMethod]
        public void ShouldRegisterAutoMapperStartupTask()
        {
            //Arrange
            var containerExtension = new StructureMapContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container.GetAllInstances<IStartupTask>();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IStartupTask>));
            Assert.IsTrue(result.Any(t => t.GetType() == typeof(AutoMapperStartupTask)));
        }


        [TestMethod]
        public void ShouldResetTheServiceLocator()
        {
            //Arrange
            var containerExtension = new StructureMapContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension).Start();

            //Act
            containerExtension.Reset();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNull(ServiceLocator.Current);
        }

        [TestMethod]
        public void ShouldSetTheContainer()
        {
            //Arrange            
            var containerExtension = new StructureMapContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = Bootstrap.Bootstrapper.GetContainer();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IContainer));
        }

        [TestMethod]
        public void ShouldResetTheContainer()
        {
            //Arrange
            var containerExtension = new StructureMapContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension).Start();

            //Act
            containerExtension.Reset();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNull(containerExtension.Container);
        }

        [TestMethod]
        public void ShouldResetTheBootstrapperContainer()
        {
            //Arrange
            var containerExtension = new StructureMapContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension).Start();

            //Act
            containerExtension.Reset();
            var result = Bootstrap.Bootstrapper.GetContainer();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNull(result);
        }
    }
}
