using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Bootstrap.Extensions.StartupTasks;
using Bootstrap.Locator;
using Bootstrap.SimpleInjector;
using Bootstrap.Tests.Extensions.TestImplementations;
using Bootstrap.Tests.Other;
using CommonServiceLocator.SimpleInjectorAdapter;
using FakeItEasy;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Bootstrap.Tests.Extensions.Containers.SimpleInjector
{
    [TestClass]
    public class SimpleInjectorExtensionTests
    {
        private IRegistrationHelper registrationHelper;

        [TestInitialize]
        public void InitializeBootstrapper()
        {
            Bootstrapper.ClearExtensions();
            registrationHelper = A.Fake<IRegistrationHelper>();
        }

        [TestMethod]
        public void Constructor_WhenInvoked_ShouldCreateASimpleInjectorExtension()
        {
            //Act
            var result = new SimpleInjectorExtension(registrationHelper);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SimpleInjectorExtension));
        }

        [TestMethod]
        public void Constructor_WhenInvoked_ShouldAddSimpleInjectorToTheExcludedAssemblies()
        {
            //Act
            var result = new SimpleInjectorExtension(registrationHelper);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Bootstrapper.Excluding.Assemblies.Contains("SimpleInjector"));
        }

        [TestMethod]
        public void Constructor_WhenInvoked_ShouldInitializeContainerToNull()
        {
            //Arrange
            var extension = new SimpleInjectorExtension(registrationHelper);

            //Act           
            var result = extension.Container;

            //Assert
            Assert.IsNull(result);            
        }

        [TestMethod]
        public void Run_WhenInvoked_ShouldInitializeContainerToAnInstanceOfSimpleInjectorContainer()
        {
            //Arrange
            var extension = new SimpleInjectorExtension(registrationHelper);

            //Act
            extension.Run();
            var result = extension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (Container));
        }

        [TestMethod]
        public void Run_WhenInvoked_ShouldRegisterAllTypesOfIBootstrapperRegistration()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(AutoMapperRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly }); 
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperRegistration>(assembly))
             .Returns(new List<Type> {typeof(AutoMapperRegistration)});
            var containerExtension = new SimpleInjectorExtension(registrationHelper);

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IBootstrapperRegistration>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperRegistration>(assembly)).MustHaveHappened();
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IBootstrapperRegistration>));
            Assert.IsTrue(result.Any());
            Assert.IsInstanceOfType(result.First(), typeof (AutoMapperRegistration));
        }

        [TestMethod]
        public void Run_WhenInvoked_ShouldRegisterAllTypesOfISimpleInjectorRegistration()
        {
            //Arrange
            var assembly = Assembly.GetExecutingAssembly();
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<ISimpleInjectorRegistration>(assembly))
             .Returns(new List<Type> { typeof(TestSimpleInjectorRegistration) });
            var containerExtension = new SimpleInjectorExtension(registrationHelper);
            
            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<ISimpleInjectorRegistration>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<ISimpleInjectorRegistration>(assembly)).MustHaveHappened();
            Assert.IsInstanceOfType(result, typeof(IEnumerable<ISimpleInjectorRegistration>));
            Assert.IsTrue(result.Any());
            Assert.IsInstanceOfType(result.First(), typeof (TestSimpleInjectorRegistration));            
        }

        [TestMethod]
        public void Run_WhenInvoked_ShouldInvokeTheRegisterMethodOfAllIBootstrapperRegistrationTypes()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(AutoMapperRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperRegistration>(assembly))
             .Returns(new List<Type> { typeof(AutoMapperRegistration) });
            A.CallTo(() => registrationHelper.GetInstancesOfTypesImplementing<IBootstrapperRegistration>())
             .Returns(new List<IBootstrapperRegistration> {new AutoMapperRegistration()});
            var containerExtension = new SimpleInjectorExtension(registrationHelper);

            //Act
            containerExtension.Run();

            //Assert
            Assert.IsNotNull(containerExtension.Resolve<IProfileExpression>());
        }

        [TestMethod]
        public void Run_WhenInvoked_ShouldInvokeTheRegisterMethodOfAllISimpleInjectorRegistrationTypes()
        {
            //Arrange
            var assembly = Assembly.GetExecutingAssembly();
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<ISimpleInjectorRegistration>(assembly))
             .Returns(new List<Type> { typeof(TestSimpleInjectorRegistration) });
            A.CallTo(() => registrationHelper.GetInstancesOfTypesImplementing<ISimpleInjectorRegistration>())
             .Returns(new List<ISimpleInjectorRegistration> { new TestSimpleInjectorRegistration() });
            var containerExtension = new SimpleInjectorExtension(registrationHelper);
            
            //Act
            containerExtension.Run();

            //Assert
            Assert.IsNotNull(containerExtension.Resolve<SimpleInjectorExtension>());            
        }

        [TestMethod]
        public void Run_WhenInvoked_ShouldSetTheContainerPropertyToASimpleInjectorContainer()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Container));
        }

        [TestMethod]
        public void Reset_WhenInvoked_ShouldSetTheContainerToNull()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper);
            containerExtension.Run();
            
            //Act
            containerExtension.Reset();

            //Assert
            Assert.IsNull(containerExtension.Container);
        }

        [TestMethod]
        public void SetServiceLocator_WhenInvokedAndContainerIsNotInitialized_ShouldThrowException()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.SetServiceLocator);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void SetServiceLocator_WhenInvoked_ShouldSetTheSimpleInjectorContainerInServiceLocator()
        {
            //Arrange
            ServiceLocator.SetLocatorProvider(() => null);
            var containerExtension = new SimpleInjectorExtension(registrationHelper);
            containerExtension.Run();

            //Act
            containerExtension.SetServiceLocator();
            var result = ServiceLocator.Current;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SimpleInjectorServiceLocatorAdapter));
        }

        [TestMethod]
        public void ResetServiceLocator_WhenInvoked_ShouldSetTheCurrentServiceLocatorToNull()
        {
            //Arrange
            ServiceLocator.SetLocatorProvider(A.Fake<IServiceLocator>);
            var containerExtension = new SimpleInjectorExtension(registrationHelper);
            containerExtension.Run();
            
            //Act
            containerExtension.ResetServiceLocator();

            //Assert
            Assert.IsNull(ServiceLocator.Current);
        }

        [TestMethod]
        public void InitializeContainer_WhenInvoked_ShouldSetTheContainerPropertyToTheContainerProvided()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper);
            var container = A.Fake<Container>();

            //Act
            containerExtension.InitializeContainer(container);
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Container));
            Assert.AreSame(result, container);

        }

        [TestMethod]
        public void Resolve_WhenInvokedAndContainerIsNotInitialized_ShouldThrowException()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.Resolve<object>());

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void Resolve_WhenInvokedWithAGenericType_ShouldResolveToASingleInstance()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper);
            var container = A.Fake<Container>();
            var instance = new object();
            container.RegisterSingle(instance);
            containerExtension.InitializeContainer(container);

            //Act
            var result = containerExtension.Resolve<object>();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreSame(instance, result);
        }

        [TestMethod]
        public void ResolveAll_WhenInvokedAndContainerIsNotInitialized_ShouldThrowException()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.ResolveAll<object>());

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ResolveAll_WhenInvokedWithAGenericType_ShouldReturnAListOfInstances()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper);
            var container = new Container();
            var instances = new IBootstrapperExtension[] {new StartupTasksExtension(A.Fake<IRegistrationHelper>()) , new ServiceLocatorExtension()};
            container.RegisterAll(instances);
            containerExtension.InitializeContainer(container);

            //Act
            var result = containerExtension.ResolveAll<IBootstrapperExtension>();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(instances.Count(), result.Count);
            Assert.IsTrue(instances.SequenceEqual(result));
        }

        [TestMethod]
        public void Register_WhenInvokedWithGeneriTagetAndImplementationTypeAndContainerIsNotInitialized_ShouldThrowException()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.Register<IBootstrapperContainerExtension, SimpleInjectorExtension>);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void Register_WhenInvokedWithGenericTargetAndImplementationType_ShouldRegisterType()
        {
            //Arrange
            var container = new Container();
            var containerExtension = new SimpleInjectorExtension(registrationHelper);
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.Register<IRegistrationHelper, RegistrationHelper>();
            var result = container.GetInstance<IRegistrationHelper>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RegistrationHelper));
        }

        [TestMethod]
        public void Register_WhenInvokedWithGenereicTypeAndInstanceAndContainerIsNotInitialized_ShouldThrowException()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.Register<IBootstrapperContainerExtension>(containerExtension));

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void Register_WhenInvokedWithGenericTypeAndInstance_ShouldRegisterInstance()
        {
            //Arrange
            var container = new Container();
            var containerExtension = new SimpleInjectorExtension(registrationHelper);
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.Register<IBootstrapperContainerExtension>(containerExtension);
            var result = container.GetInstance<IBootstrapperContainerExtension>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SimpleInjectorExtension));
            Assert.AreSame(containerExtension, result);
        }

        [TestMethod]
        public void RegisterAll_WhenInvokedWithAGenericTypeAndContainerIsNotInitialized_ShouldThrowException()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.RegisterAll<IBootstrapperContainerExtension>);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }
        
        [TestMethod]
        public void RegisterAll_WhenInvokedWithAGenericType_ShouldRegisterAllTypesThatImplementTheGenericType()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(RegistrationHelper));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IRegistrationHelper>(assembly))
             .Returns(new List<Type> {typeof (RegistrationHelper)});
            var container = new Container();
            var containerExtension = new SimpleInjectorExtension(registrationHelper);
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.RegisterAll<IRegistrationHelper>();
            var result = container.GetAllInstances<IRegistrationHelper>().ToList();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IRegistrationHelper>));
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.Any(c => c is RegistrationHelper));
        }

        [TestMethod]
        public void Options_WhenInspected_ShouldReturnABootstrapperContainerOptions()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper);

            //Act
            var result = containerExtension.Options;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(BootstrapperContainerExtensionOptions));
        }

        [TestMethod]
        public void UsingAutoRegistration_WhenSpecified_ShouldRegisterWithConventionAndWithRegistrationClasses()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestSimpleInjectorRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<ISimpleInjectorRegistration>(assembly))
             .Returns(new List<Type> { typeof(TestSimpleInjectorRegistration) });
            A.CallTo(() => registrationHelper.GetInstancesOfTypesImplementing<ISimpleInjectorRegistration>())
             .Returns(new List<ISimpleInjectorRegistration> {new TestSimpleInjectorRegistration()});
            var containerExtension = new SimpleInjectorExtension(registrationHelper);
            containerExtension.Options.UsingAutoRegistration();

            //Act
            containerExtension.Run();

            //Assert
            Assert.IsNotNull(containerExtension.Resolve<SimpleInjectorExtension>());
            Assert.IsNotNull(containerExtension.Resolve<IRegisteredByConvention>());
        }
    }
}
