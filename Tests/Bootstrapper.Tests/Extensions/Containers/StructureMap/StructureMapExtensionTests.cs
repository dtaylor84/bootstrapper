using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions.Containers;
using Bootstrap.StructureMap;
using FakeItEasy;
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
        public void ShouldCreateAStructureMapExtension()
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

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IBootstrapperRegistration>();

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

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IStructureMapRegistration>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IStructureMapRegistration>));
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIBootstrapperRegistrationTypes()
        {
            //Arrange
            var containerExtension = new StructureMapExtension();

            //Act
            containerExtension.Run();
            var result = containerExtension.Resolve<IStartupTask>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IStartupTask));
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIStructureMapRegistrationTypes()
        {
            //Arrange
            var containerExtension = new StructureMapExtension();

            //Act
            containerExtension.Run();
            var result = containerExtension.Resolve<StructureMapExtension>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StructureMapExtension));
        }

        [TestMethod]
        public void ShouldSetTheServiceLocator()
        {
            //Arrange
            Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => null);
            var containerExtension = new StructureMapExtension();
            containerExtension.Run();

            //Act            
            containerExtension.SetServiceLocator();
            var result = Microsoft.Practices.ServiceLocation.ServiceLocator.Current;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StructureMapServiceLocator));
        }

        [TestMethod]
        public void ShouldResetTheServiceLocator()
        {
            //Arrange
            var containerExtension = new StructureMapExtension();
            containerExtension.Run();

            //Act
            containerExtension.ResetServiceLocator();

            //Assert
            Assert.IsNull(Microsoft.Practices.ServiceLocation.ServiceLocator.Current);
        }

        [TestMethod]
        public void ShouldSetTheContainer()
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
        public void ShouldResetTheContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension();
            containerExtension.Run();

            //Act
            containerExtension.Reset();

            //Assert
            Assert.IsNull(containerExtension.Container);
        }

        [TestMethod]
        public void ShouldInitializeTheContainerToTheValuePassed()
        {
            //Arrange
            var containerExtension = new StructureMapExtension();
            var container = A.Fake<IContainer>();

            //Act
            containerExtension.InitializeContainer(container);
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IContainer));
            Assert.AreSame(result, container);
        }

        [TestMethod] 
        public void ShouldResolveASingleType()
        {
            //Arrange
            var containerExtension = new StructureMapExtension();
            var container = A.Fake<IContainer>();
            containerExtension.InitializeContainer(container);
            var instance = new object();
            A.CallTo(() => container.GetInstance<object>()).Returns(instance);

            //Act
            var result = containerExtension.Resolve<object>();

            //Assert
            A.CallTo(() => container.GetInstance<object>()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.AreSame(instance, result);
        }

        [TestMethod]
        public void ShouldResolveMultipleTypes()
        {
            //Arrange
            var containerExtension = new StructureMapExtension();
            var container = A.Fake<IContainer>();
            containerExtension.InitializeContainer(container);
            var instances = new List<object> { new object(), new object() };
            A.CallTo(() => container.GetAllInstances<object>()).Returns(instances);
            
            //Act
            var result = containerExtension.ResolveAll<object>();

            //Assert
            A.CallTo(() => container.GetAllInstances<object>()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.AreSame(instances, result);
        }

        [TestMethod]
        public void ShouldRegisterWithTargetAndImplementationType()
        {
            //Arrange
            var container = new Container();
            var containerExtension = new StructureMapExtension();
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.Register<IBootstrapperContainerExtension, StructureMapExtension>();
            var result = container.GetInstance<IBootstrapperContainerExtension>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StructureMapExtension));
        }

        [TestMethod]
        public void ShouldRegisterWithTargetAndImplementationInstance()
        {
            //Arrange
            var container = new Container();
            var containerExtension = new StructureMapExtension();
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.Register<IBootstrapperContainerExtension>(containerExtension);
            var result = container.GetInstance<IBootstrapperContainerExtension>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StructureMapExtension));
            Assert.AreSame(containerExtension, result);
        }

        [TestMethod]
        public void ShouldRegisterWithTargetType()
        {
            //Arrange
            var container = new Container();
            var containerExtension = new StructureMapExtension();
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.RegisterAll<IBootstrapperContainerExtension>();
            var result = container.GetAllInstances<IBootstrapperContainerExtension>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IBootstrapperContainerExtension>));
            Assert.IsTrue(result.Count() > 0);
            Assert.IsTrue(result.Any(c => c is StructureMapExtension));
        }
    }
}
