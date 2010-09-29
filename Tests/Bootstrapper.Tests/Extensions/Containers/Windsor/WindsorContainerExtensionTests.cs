using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Bootstrapper.WindsorExtension;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrapper.Tests.Extensions.Containers.Windsor
{
    [TestClass]
    public class WindsorContainerExtensionTests
    {
        [TestMethod]
        public void ShouldCreateAWindsorContainerExtension()
        {
            //Act
            var result = new WindsorContainerExtension();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WindsorContainerExtension));
        }
        
        [TestMethod]
        public void ShouldReturnANullContainer()
        {
            //Arrange
            var containerExtension = new WindsorContainerExtension();

            //Act
            var result = containerExtension.Container;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldReturnAnIWindsorContainer()
        {
            //Arrange
            var containerExtension = new WindsorContainerExtension();
            Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container;
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IWindsorContainer));
        }

        [TestMethod]
        public void ShouldRegisterAutoMapperRegistration()
        {
            //Arrange
            var containerExtension = new WindsorContainerExtension();
            Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container.ResolveAll<IWindsorRegistration>();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IWindsorRegistration>));
            Assert.IsTrue(result.Any(t => t.GetType() == typeof(AutoMapperRegistration)));
        }

        [TestMethod]
        public void ShouldRegisterStartupTaskRegistration()
        {
            //Arrange
            var containerExtension = new WindsorContainerExtension();
            Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container.ResolveAll<IWindsorRegistration>();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IWindsorRegistration>));
            Assert.IsTrue(result.Any(t => t.GetType() == typeof(StartupTaskRegistration)));
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfIWindsorRegistration()
        {
            //Arrange
            var containerExtension = new WindsorContainerExtension();
            Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container.ResolveAll<IWindsorRegistration>();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IWindsorRegistration>));
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIWindsorRegistrationTypes()
        {
            //Arrange
            var containerExtension = new WindsorContainerExtension();
            Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.LookForRegistrations.InAssembly(Assembly.GetAssembly(typeof(TestWindsorRegistration)));
            containerExtension.Run();
            var result = containerExtension.Container.Resolve<WindsorContainerExtension>();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WindsorContainerExtension));         
        }

        [TestMethod]
        public void ShouldSetTheServiceLocator()
        {
            //Arrange
            ServiceLocator.SetLocatorProvider(() => null);
            var containerExtension = new WindsorContainerExtension();
            Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = ServiceLocator.Current;
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WindsorServiceLocator));
        }

        [TestMethod]
        public void ShouldRegisterAutoMapperStartupTask()
        {
            //Arrange
            var containerExtension = new WindsorContainerExtension();
            Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container.ResolveAll<IStartupTask>();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IStartupTask>));
            Assert.IsTrue(result.Any(t => t.GetType() == typeof(AutoMapperStartupTask)));
        }


        [TestMethod]
        public void ShouldResetTheServiceLocator()
        {
            //Arrange
            var containerExtension = new WindsorContainerExtension();
            Bootstrapper.With.Container(containerExtension).Start();

            //Act
            containerExtension.Reset();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNull(ServiceLocator.Current);
        }

        [TestMethod]
        public void ShouldResetTheContainer()
        {
            //Arrange
            var containerExtension = new WindsorContainerExtension();
            Bootstrapper.With.Container(containerExtension).Start();

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
            var containerExtension = new WindsorContainerExtension();
            Bootstrapper.With.Container(containerExtension).Start();

            //Act
            containerExtension.Reset();
            var result = Bootstrapper.GetContainer();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNull(result);
        }
    }
}
