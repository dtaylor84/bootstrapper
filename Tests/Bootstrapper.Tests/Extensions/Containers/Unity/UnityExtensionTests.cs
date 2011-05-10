using System.Collections.Generic;
using Bootstrap.Extensions.Containers;
using Bootstrap.Unity;
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
            Bootstrapper.With.Extension(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container;
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IUnityContainer));
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfIUnityRegistration()
        {
            //Arrange
            var containerExtension = new UnityExtension();
            Bootstrapper.With.Extension(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IUnityRegistration>();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IUnityRegistration>));
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfIBootstrapperRegistration()
        {
            //Arrange
            var containerExtension = new UnityExtension();
            Bootstrapper.With.Extension(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IBootstrapperRegistration>();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IBootstrapperRegistration>));
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIUnityRegistrationTypes()
        {
            //Arrange
            var containerExtension = new UnityExtension();
            Bootstrapper.With.Extension(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Resolve<UnityExtension>();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnityExtension));
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIBootstrapperRegistrationTypes()
        {
            //Arrange
            var containerExtension = new UnityExtension();
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
            var containerExtension = new UnityExtension();
            Bootstrapper.With.Extension(containerExtension).Start();

            //Act
            containerExtension.SetServiceLocator();
            var result = Microsoft.Practices.ServiceLocation.ServiceLocator.Current;
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnityServiceLocator));
        }

        [TestMethod]
        public void ShouldResetTheServiceLocator()
        {
            //Arrange
            var containerExtension = new UnityExtension();
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
            var containerExtension = new UnityExtension();
            Bootstrapper.With.Extension(containerExtension);

            //Act
            containerExtension.Run();
            var result = Bootstrapper.Container;
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IUnityContainer));
        }

        [TestMethod]
        public void ShouldResetTheContainer()
        {
            //Arrange
            var containerExtension = new UnityExtension();
            Bootstrapper.With.Extension(containerExtension).Start();

            //Act
            containerExtension.Reset();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNull(containerExtension.Container);
        }

        [TestMethod]
        public void ShouldResetTheBootstrapperContainer()
        {
            //Arrange
            var containerExtension = new UnityExtension();
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
