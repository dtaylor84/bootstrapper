using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions.Containers;
using Bootstrap.StartupTasks;
using Bootstrap.Unity;
using FakeItEasy;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Containers.Unity
{
    [TestClass]
    public class UnityExtensionTests
    {
        [TestInitialize]
        public void InitializeBootstrapper()
        {
            Bootstrapper.ClearExtensions();
        }

        [TestMethod]
        public void ShouldCreateAUnityExtension()
        {
            //Act
            var result = new UnityExtension();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnityExtension));
        }

        [TestMethod]
        public void ShouldAddMicrosoftPracticesToExcludedAssemblies()
        {
            //Act
            var result = new UnityExtension();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Bootstrapper.Excluding.Assemblies.Contains("Microsoft.Practices"));
        }

        [TestMethod]
        public void ShouldReturnANullContainer()
        {
            //Arrange
            var containerExtension = new UnityExtension();

            //Act
            var result = containerExtension.Container;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldReturnAnIUnityContainer()
        {
            //Arrange
            var containerExtension = new UnityExtension();

            //Act
            containerExtension.Run();
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IUnityContainer));
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfIBootstrapperRegistration()
        {
            //Arrange
            var containerExtension = new UnityExtension();

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IBootstrapperRegistration>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IBootstrapperRegistration>));
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfIUnityRegistration()
        {
            //Arrange
            var containerExtension = new UnityExtension();

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IUnityRegistration>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IUnityRegistration>));
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIBootstrapperRegistrationTypes()
        {
            //Arrange
            var containerExtension = new UnityExtension();

            //Act
            containerExtension.Run();
            var result = containerExtension.Resolve<IStartupTask>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IStartupTask));
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIUnityRegistrationTypes()
        {
            //Arrange
            var containerExtension = new UnityExtension();

            //Act
            containerExtension.Run();
            var result = containerExtension.Resolve<UnityExtension>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnityExtension));
        }

        [TestMethod]
        public void ShouldSetTheServiceLocator()
        {
            //Arrange
            ServiceLocator.SetLocatorProvider(() => null);
            var containerExtension = new UnityExtension();
            containerExtension.Run();

            //Act
            containerExtension.SetServiceLocator();
            var result = ServiceLocator.Current;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnityServiceLocator));
        }

        [TestMethod]
        public void ShouldResetTheServiceLocator()
        {
            //Arrange
            var containerExtension = new UnityExtension();
            containerExtension.Run();

            //Act
            containerExtension.ResetServiceLocator();

            //Assert
            Assert.IsNull(ServiceLocator.Current);
        }

        [TestMethod]
        public void ShouldSetTheContainer()
        {
            //Arrange            
            var containerExtension = new UnityExtension();

            //Act
            containerExtension.Run();
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IUnityContainer));
        }

        [TestMethod]
        public void ShouldResetTheContainer()
        {
            //Arrange
            var containerExtension = new UnityExtension();
            containerExtension.Run();

            //Act
            containerExtension.Reset();

            //Assert
            Assert.IsNull(containerExtension.Container);
        }


        public void ShouldInitializeTheContainerToTheValuePassed()
        {
            //Arrange
            var containerExtension = new UnityExtension();
            var container = A.Fake<IUnityContainer>();

            //Act
            containerExtension.InitializeContainer(container);
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IUnityContainer));
            Assert.AreSame(result, container);
        }

        [TestMethod]
        public void ShouldResolveASingleUnnamedType()
        {
            //Arrange
            var containerExtension = new UnityExtension();
            var container = new UnityContainer();
            var instance1 = new object();
            var instance2 = new object();
            container.RegisterInstance(instance1);
            container.RegisterInstance("Name", instance2);
            containerExtension.InitializeContainer(container);

            //Act
            var result = containerExtension.Resolve<object>();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreSame(instance1, result);
        }

        [TestMethod]
        public void ShouldResolveASingleNamedType()
        {
            //Arrange
            var containerExtension = new UnityExtension();
            var container = new UnityContainer();
            var instance = new object();
            container.RegisterInstance("Name", instance);
            containerExtension.InitializeContainer(container);

            //Act
            var result = containerExtension.Resolve<object>();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreSame(instance, result);
        }

        [TestMethod]
        public void ShouldResolveMultipleTypes()
        {
            //Arrange
            var containerExtension = new UnityExtension();
            var container = new UnityContainer();
            var instance1 = new object();
            var instance2 = new object();
            container.RegisterInstance("Name1", instance1);
            container.RegisterInstance("Name2", instance2);
            containerExtension.InitializeContainer(container);

            //Act
            var result = containerExtension.ResolveAll<object>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(o => o == instance1));
            Assert.IsTrue(result.Any(o => o == instance2));
        }

        [TestMethod]
        public void ShouldRegisterWithTargetAndImplementationType()
        {
            //Arrange
            var container = new UnityContainer();
            var containerExtension = new UnityExtension();
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.Register<IBootstrapperContainerExtension, UnityExtension>();
            var result = container.Resolve<IBootstrapperContainerExtension>(typeof(UnityExtension).Name);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnityExtension));
        }

        [TestMethod]
        public void ShouldRegisterWithTargetAndImplementationInstance()
        {
            //Arrange
            var container = new UnityContainer();
            var containerExtension = new UnityExtension();
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.Register<IBootstrapperContainerExtension>(containerExtension);
            var result = container.Resolve<IBootstrapperContainerExtension>(typeof(UnityExtension).Name);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnityExtension));
            Assert.AreSame(containerExtension, result);
        }

        [TestMethod]
        public void ShouldRegisterWithTargetType()
        {
            //Arrange
            var container = new UnityContainer();
            var containerExtension = new UnityExtension();
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.RegisterAll<IBootstrapperContainerExtension>();
            var result = container.ResolveAll<IBootstrapperContainerExtension>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IBootstrapperContainerExtension>));
            Assert.IsTrue(result.Count() > 0);
            Assert.IsTrue(result.Any(c => c is UnityExtension));
        }

        [TestMethod]
        public void ShouldReturnABootstrapperContainerExtensionOptions()
        {
            //Arrange
            var containerExtension = new UnityExtension();

            //Act
            var result = containerExtension.Options;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(BootstrapperContainerExtensionOptions));
        }
    }
}
