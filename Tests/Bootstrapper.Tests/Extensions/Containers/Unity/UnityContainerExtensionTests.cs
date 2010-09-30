using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bootstrap.UnityExtension;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityContainerExtension = Bootstrap.UnityExtension.UnityContainerExtension;

namespace Bootstrap.Tests.Extensions.Containers.Unity
{
    [TestClass]
    public class UnityContainerExtensionTests
    {
        [TestMethod]
        public void ShouldCreateAUnityContainerExtension()
        {
            //Act
            var result = new UnityContainerExtension();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnityContainerExtension));
        }

        [TestMethod]
        public void ShouldReturnANullContainer()
        {
            //Arrange
            var containerExtension = new UnityContainerExtension();

            //Act
            var result = containerExtension.Container;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldReturnAnIUnityContainer()
        {
            //Arrange
            var containerExtension = new UnityContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container;
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IUnityContainer));
        }

        [TestMethod]
        public void ShouldRegisterAutoMapperRegistration()
        {
            //Arrange
            var containerExtension = new UnityContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container.ResolveAll<IUnityRegistration>();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IUnityRegistration>));
            Assert.IsTrue(result.Any(t => t.GetType() == typeof(AutoMapperRegistration)));
        }

        [TestMethod]
        public void ShouldRegisterStartupTaskRegistration()
        {
            //Arrange
            var containerExtension = new UnityContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container.ResolveAll<IUnityRegistration>();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IUnityRegistration>));
            Assert.IsTrue(result.Any(t => t.GetType() == typeof(StartupTaskRegistration)));
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfIUnityRegistration()
        {
            //Arrange
            var containerExtension = new UnityContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container.ResolveAll<IUnityRegistration>();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IUnityRegistration>));
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIUnityRegistrationTypes()
        {
            //Arrange
            var containerExtension = new UnityContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.LookForRegistrations.InAssembly(Assembly.GetAssembly(typeof(TestUnityRegistration)));
            containerExtension.Run();
            var result = containerExtension.Container.Resolve<UnityContainerExtension>();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnityContainerExtension));
        }

        [TestMethod]
        public void ShouldSetTheServiceLocator()
        {
            //Arrange
            ServiceLocator.SetLocatorProvider(() => null);
            var containerExtension = new UnityContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = ServiceLocator.Current;
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnityServiceLocator));
        }

        [TestMethod]
        public void ShouldRegisterAutoMapperStartupTask()
        {
            //Arrange
            var containerExtension = new UnityContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container.ResolveAll<IStartupTask>();
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
            var containerExtension = new UnityContainerExtension();
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
            var containerExtension = new UnityContainerExtension();
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = Bootstrap.Bootstrapper.GetContainer();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IUnityContainer));
        }


        [TestMethod]
        public void ShouldResetTheContainer()
        {
            //Arrange
            var containerExtension = new UnityContainerExtension();
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
            var containerExtension = new UnityContainerExtension();
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
