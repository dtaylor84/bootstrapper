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
using Shouldly;
using SimpleInjector;

namespace Bootstrap.Tests.Extensions.Containers.SimpleInjector
{
    using global::AutoMapper.QueryableExtensions;

    [TestClass]
    public class SimpleInjectorExtensionTests
    {
        private IRegistrationHelper registrationHelper;
        private IBootstrapperContainerExtensionOptions options;

        [TestInitialize]
        public void InitializeBootstrapper()
        {
            Bootstrapper.ClearExtensions();
            registrationHelper = A.Fake<IRegistrationHelper>();
            options = A.Fake<IBootstrapperContainerExtensionOptions>();
        }

        [TestMethod]
        public void Constructor_WhenInvoked_ShouldCreateASimpleInjectorExtension()
        {
            //Act
            var result = new SimpleInjectorExtension(registrationHelper, options);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SimpleInjectorExtension));
        }

        [TestMethod]
        public void Constructor_WhenInvoked_ShouldAddSimpleInjectorToTheExcludedAssemblies()
        {
            //Act
            var result = new SimpleInjectorExtension(registrationHelper, options);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Bootstrapper.Excluding.Assemblies.Contains("SimpleInjector"));
        }

        [TestMethod]
        public void Constructor_WhenInvoked_ShouldInitializeContainerToNull()
        {
            //Arrange
            var extension = new SimpleInjectorExtension(registrationHelper, options);

            //Act           
            var result = extension.Container;

            //Assert
            Assert.IsNull(result);            
        }

        [TestMethod]
        public void Run_WhenInvoked_ShouldInitializeContainerToAnInstanceOfSimpleInjectorContainer()
        {
            //Arrange
            var extension = new SimpleInjectorExtension(registrationHelper, options);

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
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);

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
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);
            
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
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);

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
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);
            
            //Act
            containerExtension.Run();

            //Assert
            Assert.IsNotNull(containerExtension.Resolve<SimpleInjectorExtension>());            
        }

        [TestMethod]
        public void Run_WhenInvoked_ShouldSetTheContainerPropertyToASimpleInjectorContainer()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);

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
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);
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
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);

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
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);
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
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);
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
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);
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
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.Resolve<object>());

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void Resolve_WhenInvokedWithAGenericType_ShouldResolveToASingleInstance()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);
            var container = A.Fake<Container>();
            var instance = new object();
            container.RegisterSingleton(instance);
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
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.ResolveAll<object>());

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ResolveAll_WhenInvokedWithAGenericType_ShouldReturnAListOfInstances()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);
            var container = new Container();
            var instances = new IBootstrapperExtension[] {new StartupTasksExtension(A.Fake<IRegistrationHelper>()) , new ServiceLocatorExtension()};
            container.RegisterCollection(instances);
            containerExtension.InitializeContainer(container);

            //Act
            var result = containerExtension.ResolveAll<IBootstrapperExtension>();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(instances.Count(), result.Count);
            Assert.IsTrue(instances.SequenceEqual(result));
        }

        [TestMethod]
        public void Register_WhenInvokedWithNonGenericTargetAndContainerIsNotInitialized_ShouldThrowException()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.RegisterAll(typeof(IGenericTest<>)));

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void Register_WhenInvokedWithGenericTargetAndImplementationTypeAndContainerIsNotInitialized_ShouldThrowException()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.Register<IBootstrapperContainerExtension, SimpleInjectorExtension>);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void RegisterAll_WhenInvokedWithNonGenericTargetType_ShouldRegisterType()
        {
            //Arrange
            var container = new Container();
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);
            containerExtension.InitializeContainer(container);
            var thisAssembly = Assembly.GetAssembly(typeof (GenericTest<>));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new[] { thisAssembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing(thisAssembly, typeof(IGenericTest<>)))
                .Returns(new[] { typeof(GenericTest<>) });

            //Act
            containerExtension.RegisterAll(typeof(IGenericTest<>));
            var result1 = container.GetInstance<IGenericTest<object>>();
            var result2 = container.GetInstance<IGenericTest<string[]>>();

            //Assert
            result1.ShouldBeOfType<GenericTest<object>>();
            result2.ShouldBeOfType<GenericTest<string[]>>();
        }

        [TestMethod]
        public void Register_WhenInvokedWithGenericTargetAndImplementationType_ShouldRegisterType()
        {
            //Arrange
            var container = new Container();
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.Register<IBootstrapperAssemblyProvider, LoadedAssemblyProvider>();
            var result = container.GetInstance<IBootstrapperAssemblyProvider>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(LoadedAssemblyProvider));
        }

        [TestMethod]
        public void Register_WhenInvokedWithGenereicTypeAndInstanceAndContainerIsNotInitialized_ShouldThrowException()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);

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
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);
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
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);

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
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperAssemblyProvider>(assembly))
             .Returns(new List<Type> {typeof (LoadedAssemblyProvider), typeof(ReferencedAssemblyProvider)});
            var container = new Container();
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.RegisterAll<IBootstrapperAssemblyProvider>();
            var result = container.GetAllInstances<IBootstrapperAssemblyProvider>().ToList();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IBootstrapperAssemblyProvider>));
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.Any(c => c is LoadedAssemblyProvider));
            Assert.IsTrue(result.Any(c => c is ReferencedAssemblyProvider));
        }

        [TestMethod]
        public void Options_WhenInspected_ShouldReturnASimpleInjectorOptions()
        {
            //Arrange
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);

            //Act
            var result = containerExtension.Options;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(SimpleInjectorOptions));
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
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);
            A.CallTo(() => options.AutoRegistration).Returns(true);

            //Act
            containerExtension.Run();

            //Assert
            Assert.IsNotNull(containerExtension.Resolve<SimpleInjectorExtension>());
            Assert.IsNotNull(containerExtension.Resolve<IRegisteredByConvention>());
        }

        [TestMethod]
        public void Run_WhenTheContainerInOptionsIsSet_ShouldUseTheExistingContainer()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestSimpleInjectorRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<ISimpleInjectorRegistration>(assembly))
                .Returns(new List<Type> { typeof(TestSimpleInjectorRegistration) });
            var container = new Container();
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options)
            {
                Options = { Container = container }
            };


            //Act
            containerExtension.Run();

            //Assert
            Assert.AreSame(container, containerExtension.Container);
        }

        [TestMethod]
        public void Run_WhenInvokedAndAutoMapperExtensionIsLoaded_ShouldRegisterMapperAsSingelton()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(AutoMapperRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperRegistration>(assembly))
                .Returns(new List<Type> { typeof(AutoMapperRegistration) });
            A.CallTo(() => registrationHelper.GetInstancesOfTypesImplementing<IBootstrapperRegistration>())
                .Returns(new List<IBootstrapperRegistration> {new AutoMapperRegistration()});
            var containerExtension = new SimpleInjectorExtension(registrationHelper, options);

            //Act
            containerExtension.Run();

            //Assert
            Assert.AreSame(AutoMapperExtension.ConfigurationProvider, containerExtension.Resolve<IConfigurationProvider>());
            Assert.AreSame(AutoMapperExtension.ProfileExpression, containerExtension.Resolve<IProfileExpression>());
            Assert.AreSame(AutoMapperExtension.Mapper, containerExtension.Resolve<IMapper>());
            Assert.AreSame(AutoMapperExtension.Engine, containerExtension.Resolve<IMappingEngine>());
            Assert.AreSame(containerExtension.Resolve<IConfigurationProvider>(), containerExtension.Resolve<IConfigurationProvider>());
            Assert.AreSame(containerExtension.Resolve<IProfileExpression>(), containerExtension.Resolve<IProfileExpression>());
            Assert.AreSame(containerExtension.Resolve<IMapper>(), containerExtension.Resolve<IMapper>());
            Assert.AreSame(containerExtension.Resolve<IMappingEngine>(), containerExtension.Resolve<IMappingEngine>());
        }
    }
}
