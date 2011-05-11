using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions.Containers;
using Bootstrap.Windsor;
using CommonServiceLocator.WindsorAdapter;
using FakeItEasy;
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

            //Act
            containerExtension.Run();
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IWindsorContainer));
        }
        
        [TestMethod]
        public void ShouldRegisterAllTypesOfIBootstrapperRegistration()
        {
            //Arrange
            var containerExtension = new WindsorExtension();

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IBootstrapperRegistration>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IBootstrapperRegistration>));
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfIWindsorRegistration()
        {
            //Arrange
            var containerExtension = new WindsorExtension();

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IWindsorRegistration>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IWindsorRegistration>));
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIBootstrapperRegistrationTypes()
        {
            //Arrange
            var containerExtension = new WindsorExtension();

            //Act
            containerExtension.Run();
            var result = containerExtension.Resolve<IStartupTask>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IStartupTask));
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIWindsorRegistrationTypes()
        {
            //Arrange
            var containerExtension = new WindsorExtension();

            //Act
            containerExtension.Run();
            var result = containerExtension.Resolve<WindsorExtension>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WindsorExtension));         
        }

        [TestMethod]
        public void ShouldSetTheServiceLocator()
        {
            //Arrange
            Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => null);
            var containerExtension = new WindsorExtension();
            containerExtension.Run();

            //Act
            containerExtension.SetServiceLocator();
            var result = Microsoft.Practices.ServiceLocation.ServiceLocator.Current;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WindsorServiceLocator));
        }

        [TestMethod]
        public void ShouldResetTheServiceLocator()
        {
            //Arrange
            var containerExtension = new WindsorExtension();
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
            var containerExtension = new WindsorExtension();

            //Act
            containerExtension.Run();
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IWindsorContainer));
        }

        [TestMethod]
        public void ShouldResetTheContainer()
        {
            //Arrange
            var containerExtension = new WindsorExtension();
            containerExtension.Run();

            //Act
            containerExtension.Reset();

            //Assert
            Assert.IsNull(Bootstrapper.ContainerExtension);
        }

        [TestMethod]
        public void ShouldInitializeTheContainerToTheValuePassed()
        {
            //Arrange
            var containerExtension = new WindsorExtension();
            var container = A.Fake<IWindsorContainer>();

            //Act
            containerExtension.InitializeContainer(container);
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IWindsorContainer));
            Assert.AreSame(result, container);
        }

        [TestMethod]
        public void ShouldResolveASingleType()
        {
            //Arrange
            var containerExtension = new WindsorExtension();
            var container = A.Fake<IWindsorContainer>();
            containerExtension.InitializeContainer(container);
            var instance = new object();
            A.CallTo(() => container.Resolve<object>()).Returns(instance);

            //Act
            var result = containerExtension.Resolve<object>();

            //Assert
            A.CallTo(() => container.Resolve<object>()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.AreSame(instance, result);
        }

        [TestMethod]
        public void ShouldResolveMultipleTypes()
        {
            //Arrange
            var containerExtension = new WindsorExtension();
            var container = A.Fake<IWindsorContainer>();
            containerExtension.InitializeContainer(container);
            var instances = new [] { new object(), new object() };
            A.CallTo(() => container.ResolveAll<object>()).Returns(instances);

            //Act
            var result = containerExtension.ResolveAll<object>();

            //Assert
            A.CallTo(() => container.ResolveAll<object>()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.AreSame(instances, result);
        }

        [TestMethod]
        public void ShouldRegisterWithTargetAndImplementationType()
        {
            //Arrange
            var container = new WindsorContainer();
            var containerExtension = new WindsorExtension();
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.Register<IBootstrapperContainerExtension, WindsorExtension>();
            var result = container.Resolve<IBootstrapperContainerExtension>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WindsorExtension));
        }

        [TestMethod]
        public void ShouldRegisterWithTargetAndImplementationInstance()
        {
            //Arrange
            var container = new WindsorContainer();
            var containerExtension = new WindsorExtension();
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.Register<IBootstrapperContainerExtension>(containerExtension);
            var result = container.Resolve<IBootstrapperContainerExtension>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WindsorExtension));
            Assert.AreSame(containerExtension, result);
        }

        [TestMethod]
        public void ShouldRegisterWithTargetType()
        {
            //Arrange
            var container = new WindsorContainer();
            var containerExtension = new WindsorExtension();
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.RegisterAll<IBootstrapperContainerExtension>();
            var result = container.ResolveAll<IBootstrapperContainerExtension>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IBootstrapperContainerExtension>));
            Assert.IsTrue(result.Count() > 0);
            Assert.IsTrue(result.Any(c => c is WindsorExtension));
        }
    }
}
