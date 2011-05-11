using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions.Containers;
using Bootstrap.StructureMap;
using StructureMap;
using StructureMap.ServiceLocatorAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Containers.StructureMap
{
    [TestClass]
    public class StructureMapExtensionTests
    {
        [TestInitialize]
        public void InitializeBootstrapper()
        {
            Bootstrapper.ClearExtensions();
        }

        [TestMethod]
        public void ShouldCreateAStructureMapContainerExtension()
        {
            //Act
            var result = new StructureMapExtension();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StructureMapExtension));
        }

        [TestMethod]
        public void ShouldReturnANullContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension();

            //Act
            var result = containerExtension.Container;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldReturnAnIContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension();

            //Act
            containerExtension.Run();
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IContainer));
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfIBootstrapperRegistration()
        {
            //Arrange
            var containerExtension = new StructureMapExtension();
            Bootstrapper.With.Extension(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IBootstrapperRegistration>();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IBootstrapperRegistration>));
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfIStructureMapRegistration()
        {
            //Arrange
            var containerExtension = new StructureMapExtension();
            Bootstrapper.With.Extension(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IStructureMapRegistration>();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IStructureMapRegistration>));
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIStructureMapRegistrationTypes()
        {
            //Arrange
            var containerExtension = new StructureMapExtension();
            Bootstrapper.With.Extension(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Resolve<StructureMapExtension>();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StructureMapExtension));
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIBootstrapperRegistrationTypes()
        {
            //Arrange
            var containerExtension = new StructureMapExtension();
            Bootstrapper.With.Extension(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Resolve<IStartupTask>();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IStartupTask));
        }


        [TestMethod]
        public void ShouldSetTheServiceLocator()
        {
            //Arrange
            Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => null);
            var containerExtension = new StructureMapExtension();
            Bootstrapper.With.Extension(containerExtension).Start();

            //Act
            containerExtension.SetServiceLocator();
            var result = Microsoft.Practices.ServiceLocation.ServiceLocator.Current;
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StructureMapServiceLocator));
        }

        [TestMethod]
        public void ShouldResetTheServiceLocator()
        {
            //Arrange
            var containerExtension = new StructureMapExtension();
            Bootstrapper.With.Extension(containerExtension).Start();

            //Act
            containerExtension.ResetServiceLocator();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNull(Microsoft.Practices.ServiceLocation.ServiceLocator.Current);
        }

        [TestMethod]
        public void ShouldSetTheContainer()
        {
            //Arrange            
            var containerExtension = new StructureMapExtension();
            Bootstrapper.With.Extension(containerExtension);

            //Act
            containerExtension.Run();
            var result = Bootstrapper.Container;
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IContainer));
        }

        [TestMethod]
        public void ShouldResetTheContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension();
            Bootstrapper.With.Extension(containerExtension).Start();

            //Act
            containerExtension.Reset();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNull(Bootstrapper.Container);
        }

        [TestMethod]
        public void ShouldResetTheBootstrapperContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension();
            Bootstrapper.With.Extension(containerExtension).Start();

            //Act
            containerExtension.Reset();
            var result = Bootstrapper.Container;
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNull(result);
        }
    }
}
