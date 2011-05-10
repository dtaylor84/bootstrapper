using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions.Containers;
using Bootstrap.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.Windsor;

namespace Bootstrap.Tests.Extensions.Containers.Windsor
{
    [TestClass]
    public class WindsorExtensionTests
    {
        [TestInitialize]
        public void InitializeBootstrapper()
        {
            Bootstrapper.ClearExtensions();        
        }

        [TestMethod]
        public void ShouldCreateAWindsorContainerExtension()
        {
            //Act
            var result = new WindsorExtension();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WindsorExtension));
        }
        
        [TestMethod]
        public void ShouldReturnANullContainer()
        {
            //Arrange
            var containerExtension = new WindsorExtension();

            //Act
            var result = containerExtension.Container;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldReturnAnIWindsorContainer()
        {
            //Arrange
            var containerExtension = new WindsorExtension();
            Bootstrapper.With.Extension(containerExtension);

            //Act
            containerExtension.Run();
            var result = Bootstrapper.Container;
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IWindsorContainer));
        }
        
        [TestMethod]
        public void ShouldRegisterAllTypesOfIWindsorRegistration()
        {
            //Arrange
            var containerExtension = new WindsorExtension();
            Bootstrapper.With.Extension(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IWindsorRegistration>();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IWindsorRegistration>));
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfIBootstrapperRegistration()
        {
            //Arrange
            var containerExtension = new WindsorExtension();
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
        public void ShouldInvokeTheRegisterMethodOfAllIWindsorRegistrationTypes()
        {
            //Arrange
            var containerExtension = new WindsorExtension();
            Bootstrapper.With.Extension(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Resolve<WindsorExtension>();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WindsorExtension));         
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIBootstrapperRegistrationTypes()
        {
            //Arrange
            var containerExtension = new WindsorExtension();
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
            var containerExtension = new WindsorExtension();
            Bootstrapper.With.Extension(containerExtension).Start();

            //Act
            containerExtension.SetServiceLocator();
            var result = Microsoft.Practices.ServiceLocation.ServiceLocator.Current;
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WindsorServiceLocator));
        }

        [TestMethod]
        public void ShouldResetTheServiceLocator()
        {
            //Arrange
            var containerExtension = new WindsorExtension();
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
            var containerExtension = new WindsorExtension();
            Bootstrapper.With.Extension(containerExtension);

            //Act
            containerExtension.Run();
            var result = Bootstrapper.Container;
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IWindsorContainer));
        }

        [TestMethod]
        public void ShouldResetTheContainer()
        {
            //Arrange
            var containerExtension = new WindsorExtension();
            Bootstrapper.With.Extension(containerExtension).Start();

            //Act
            containerExtension.Reset();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNull(Bootstrapper.ContainerExtension);
        }

        [TestMethod]
        public void ShouldResetTheBootstrapperContainer()
        {
            //Arrange
            var containerExtension = new WindsorExtension();
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
