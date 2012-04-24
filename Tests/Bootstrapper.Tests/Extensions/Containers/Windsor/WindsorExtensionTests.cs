using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions.Containers;
using Bootstrap.Tests.Extensions.TestImplementations;
using Bootstrap.Tests.Other;
using Bootstrap.Windsor;
using Castle.MicroKernel;
using CommonServiceLocator.WindsorAdapter;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceLocation;
using Castle.Windsor;

namespace Bootstrap.Tests.Extensions.Containers.Windsor
{
    [TestClass]
    public class WindsorExtensionTests
    {
        private IRegistrationHelper registrationHelper;

        [TestInitialize]
        public void InitializeBootstrapper()
        {
            Bootstrapper.ClearExtensions();
            registrationHelper = A.Fake<IRegistrationHelper>();
        }

        [TestMethod]
        public void ShouldCreateAWindsorContainerExtension()
        {
            //Act
            var result = new WindsorExtension(registrationHelper);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WindsorExtension));
        }

        [TestMethod]
        public void ShouldAddCastleToExcludedAssemblies()
        {
            //Act
            var result = new WindsorExtension(registrationHelper);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Bootstrapper.Excluding.Assemblies.Contains("Castle"));
            Assert.IsTrue(Bootstrapper.Excluding.Assemblies.Contains("Castle.Facilities.FactorySupport"));
        }
        
        [TestMethod]
        public void ShouldReturnANullContainer()
        {
            //Arrange
            var containerExtension = new WindsorExtension(registrationHelper);

            //Act
            var result = containerExtension.Container;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldReturnAnIWindsorContainer()
        {
            //Arrange
            var containerExtension = new WindsorExtension(registrationHelper);

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
            var assembly = Assembly.GetAssembly(typeof(AutoMapperRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly }); 
            var containerExtension = new WindsorExtension(registrationHelper);

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IBootstrapperRegistration>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IBootstrapperRegistration>));
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result[0] is AutoMapperRegistration);
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfIWindsorRegistration()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestWindsorRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });
            var containerExtension = new WindsorExtension(registrationHelper);

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IWindsorRegistration>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IWindsorRegistration>));
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIBootstrapperRegistrationTypes()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(AutoMapperRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });
            var containerExtension = new WindsorExtension(registrationHelper);

            //Act
            containerExtension.Run();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            Assert.IsNotNull(containerExtension.Resolve<IProfileExpression>());
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIWindsorRegistrationTypes()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestWindsorRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });
            var containerExtension = new WindsorExtension(registrationHelper);

            //Act
            containerExtension.Run();
            var result = containerExtension.Resolve<WindsorExtension>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WindsorExtension));         
        }

        [TestMethod]
        public void ShouldSetTheServiceLocator()
        {
            //Arrange
            ServiceLocator.SetLocatorProvider(() => null);
            var containerExtension = new WindsorExtension(registrationHelper);
            containerExtension.Run();

            //Act
            containerExtension.SetServiceLocator();
            var result = ServiceLocator.Current;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WindsorServiceLocator));
        }

        [TestMethod]
        public void ShouldResetTheServiceLocator()
        {
            //Arrange
            var containerExtension = new WindsorExtension(registrationHelper);
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
            var containerExtension = new WindsorExtension(registrationHelper);

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
            var containerExtension = new WindsorExtension(registrationHelper);
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
            var containerExtension = new WindsorExtension(registrationHelper);
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
            var containerExtension = new WindsorExtension(registrationHelper);
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
            var containerExtension = new WindsorExtension(registrationHelper);
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
            var containerExtension = new WindsorExtension(registrationHelper);
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.Register<IRegistrationHelper, RegistrationHelper>();
            var result = container.Resolve<IRegistrationHelper>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RegistrationHelper));
        }

        [TestMethod]
        public void ShouldRegisterWithTargetAndImplementationInstance()
        {
            //Arrange
            var container = new WindsorContainer();
            var containerExtension = new WindsorExtension(registrationHelper);
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
            var assembly = Assembly.GetAssembly(typeof(RegistrationHelper));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });
            var container = new WindsorContainer();
            var containerExtension = new WindsorExtension(registrationHelper);
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.RegisterAll<IRegistrationHelper>();
            var result = container.ResolveAll<IRegistrationHelper>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IRegistrationHelper>));
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.Any(c => c is RegistrationHelper));
        }

        [TestMethod]
        public void ShouldReturnABootstrapperContainerExtensionOptions()
        {
            //Arrange
            var containerExtension = new WindsorExtension(registrationHelper);

            //Act
            var result = containerExtension.Options;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(BootstrapperContainerExtensionOptions));
        }

        [TestMethod]
        public void ShouldRegisterWithConventionAndWithRegistration()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestWindsorRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });
            var containerExtension = new WindsorExtension(registrationHelper);
            containerExtension.Options.UsingAutoRegistration();

            //Act
            containerExtension.Run();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            Assert.IsNotNull(containerExtension.Resolve<WindsorExtension>());
            Assert.IsNotNull(containerExtension.Resolve<IRegisteredByConvention>());
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenSettingServiceLocatorBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new WindsorExtension(registrationHelper);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.SetServiceLocator);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenResolvingSimpleTypeBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new WindsorExtension(registrationHelper);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.Resolve<object>());

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenResolvingMultipleTypesBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new WindsorExtension(registrationHelper);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.ResolveAll<object>());

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenRegisteringWithTargetAndImplementationTypeBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new WindsorExtension(registrationHelper);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.Register<IBootstrapperContainerExtension, WindsorExtension>);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenRegisteringWithTargetAndImplementationInstanceBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new WindsorExtension(registrationHelper);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.Register<IBootstrapperContainerExtension>(containerExtension));

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenRegisteringWithTargetTypeBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new WindsorExtension(registrationHelper);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.RegisterAll<IBootstrapperContainerExtension>);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldAddFacility()
        {
            // Arrange
            var containerExtension = new WindsorExtension(registrationHelper);
            var facility = A.Fake<IFacility>();
            var container = new WindsorContainer();

            // Act
            containerExtension.InitializeContainer(container);
            containerExtension.AddFacility(facility);

            // Assert
            Assert.IsTrue(container.Kernel.GetFacilities().Contains(facility));
        }
    }
}
