using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions.Containers;
using Bootstrap.StructureMap;
using Bootstrap.Tests.Extensions.TestImplementations;
using Bootstrap.Tests.Other;
using CommonServiceLocator.StructureMapAdapter.Unofficial;
using FakeItEasy;
using Shouldly;
using StructureMap;
using StructureMap.Configuration.DSL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceLocation;
using System.Reflection;

namespace Bootstrap.Tests.Extensions.Containers.StructureMap
{
    [TestClass]
    public class StructureMapExtensionTests
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
        public void ShouldCreateAStructureMapExtension()
        {
            //Act
            var result = new StructureMapExtension(registrationHelper, options);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StructureMapExtension));
        }

        [TestMethod]
        public void ShouldAddStructureMapToExcludedAssemblies()
        {
            //Act
            var result = new StructureMapExtension(registrationHelper, options);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Bootstrapper.Excluding.Assemblies.Contains("StructureMap"));
        }

        [TestMethod]
        public void ShouldReturnANullContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension(registrationHelper, options);

            //Act
            var result = containerExtension.Container;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldReturnAnIContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension(registrationHelper, options);

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
            var assembly = Assembly.GetAssembly(typeof (AutoMapperRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> {assembly});
            var containerExtension = new StructureMapExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IBootstrapperRegistration>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IBootstrapperRegistration>));
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfIStructureMapRegistration()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestStructureMapRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });
            var containerExtension = new StructureMapExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IStructureMapRegistration>();

            //Assert            
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IStructureMapRegistration>));
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfRegistry()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestStructureMapRegistry));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });
            var containerExtension = new StructureMapExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<Registry>();

            //Assert            
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Registry>));
            Assert.IsTrue(result.Any());
        }


        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIBootstrapperRegistrationTypes()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(AutoMapperRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });
            var containerExtension = new StructureMapExtension(registrationHelper, options);

            //Act
            containerExtension.Run();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            Assert.IsNotNull(containerExtension.Resolve<IProfileExpression>());
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIStructureMapRegistrationTypes()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestStructureMapRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });
            var containerExtension = new StructureMapExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.Resolve<StructureMapExtension>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StructureMapExtension));
        }

        [TestMethod]
        public void ShouldInvokeRegistrationsOfAllRegistryTypes()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestStructureMapRegistry));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });
            var containerExtension = new StructureMapExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.Resolve<ITestInterface>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ITestInterface));
        }


        [TestMethod]
        public void ShouldSetTheServiceLocator()
        {
            //Arrange
            ServiceLocator.SetLocatorProvider(() => null);
            var containerExtension = new StructureMapExtension(registrationHelper, options);
            containerExtension.Run();

            //Act            
            containerExtension.SetServiceLocator();
            var result = ServiceLocator.Current;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StructureMapServiceLocator));
        }

        [TestMethod]
        public void ShouldResetTheServiceLocator()
        {
            //Arrange
            ServiceLocator.SetLocatorProvider(A.Fake<IServiceLocator>);
            var containerExtension = new StructureMapExtension(registrationHelper, options);
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
            var containerExtension = new StructureMapExtension(registrationHelper, options);

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
            var containerExtension = new StructureMapExtension(registrationHelper, options);
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
            var containerExtension = new StructureMapExtension(registrationHelper, options);
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
            var containerExtension = new StructureMapExtension(registrationHelper, options);
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
            var containerExtension = new StructureMapExtension(registrationHelper, options);
            var container = A.Fake<IContainer>();
            containerExtension.InitializeContainer(container);
            var instances = new List<object> { new object(), new object() };
            A.CallTo(() => container.GetAllInstances<object>()).Returns(instances);
            
            //Act
            var result = containerExtension.ResolveAll<object>();

            //Assert
            A.CallTo(() => container.GetAllInstances<object>()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.AreEqual(instances.Count, result.Count);
            Assert.AreSame(instances[0], result[0]);
            Assert.AreSame(instances[1], result[1]);
        }

        [TestMethod]
        public void RegisterAll_WhenInvokedWithNonGenericTargetType_ShouldRegisterType()
        {
            //Arrange
            var container = new Container();
            var containerExtension = new StructureMapExtension(registrationHelper, options);
            containerExtension.InitializeContainer(container);
            var thisAssembly = Assembly.GetAssembly(typeof (GenericTest<>));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new[] { thisAssembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing(thisAssembly, typeof (IGenericTest<>)))
                .Returns(new[] {typeof (GenericTest<>)});


            //Act
            containerExtension.RegisterAll(typeof(IGenericTest<>));
            var result1 = container.GetInstance<IGenericTest<object>>();
            var result2 = container.GetInstance<IGenericTest<string[]>>();

            //Assert
            result1.ShouldBeOfType<GenericTest<object>>();
            result2.ShouldBeOfType<GenericTest<string[]>>();
        }

        [TestMethod]
        public void ShouldRegisterWithTargetAndImplementationType()
        {
            //Arrange
            var container = new Container();
            var containerExtension = new StructureMapExtension(registrationHelper, options);
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.Register<IBootstrapperAssemblyProvider, LoadedAssemblyProvider>();
            var result = container.GetInstance<IBootstrapperAssemblyProvider>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(LoadedAssemblyProvider));
        }

        [TestMethod]
        public void ShouldRegisterWithTargetAndImplementationInstance()
        {
            //Arrange
            var container = new Container();
            var containerExtension = new StructureMapExtension(registrationHelper, options);
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
            var assembly = Assembly.GetAssembly(typeof(RegistrationHelper));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperAssemblyProvider>(assembly))
             .Returns(new List<Type> {typeof (LoadedAssemblyProvider), typeof (ReferencedAssemblyProvider)});
            var container = new Container();
            var containerExtension = new StructureMapExtension(registrationHelper, options);
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.RegisterAll<IBootstrapperAssemblyProvider>();
            var result = container.GetAllInstances<IBootstrapperAssemblyProvider>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IBootstrapperAssemblyProvider>));
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.Any(c => c is LoadedAssemblyProvider));
            Assert.IsTrue(result.Any(c => c is ReferencedAssemblyProvider));
        }

        [TestMethod]
        public void ShouldReturnABootstrapperContainerExtensionOptions()
        {
            //Arrange
            var containerExtension = new StructureMapExtension(registrationHelper, options);

            //Act
            var result = containerExtension.Options;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(StructureMapOptions));
        }

        [TestMethod]
        public void ShouldRegisterWithConventionAndWithRegistration()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(StructureMapExtensionTests));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });

            var containerExtension = new StructureMapExtension(registrationHelper, options);
            A.CallTo(() => options.AutoRegistration).Returns(true);

            //Act
            containerExtension.Run();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            Assert.IsNotNull(containerExtension.Resolve<StructureMapExtension>());
            Assert.IsNotNull(containerExtension.Resolve<IRegisteredByConvention>());
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenSettingServiceLocatorBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.SetServiceLocator);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenResolvingSimpleTypeBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.Resolve<object>());

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod] public void ShouldThrowNoContainerExceptionWhenResolvingMultipleTypesBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.ResolveAll<object>());

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void Register_WhenInvokedWithNonGenericTargetAndContainerIsNotInitialized_ShouldThrowException()
        {
            //Arrange
            var containerExtension = new StructureMapExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.RegisterAll(typeof(IGenericTest<>)));

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenRegisteringWithTargetAndImplementationTypeBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.Register<IBootstrapperContainerExtension, StructureMapExtension>);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenRegisteringWithTargetAndImplementationInstanceBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.Register<IBootstrapperContainerExtension>(containerExtension));

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenRegisteringWithTargetTypeBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.RegisterAll<IBootstrapperContainerExtension>);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void Run_WhenTheContainerInOptionsIsSet_ShouldUseTheExistingContainer()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestStructureMapRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IStructureMapRegistration>(assembly))
                .Returns(new List<Type> { typeof(TestStructureMapRegistration) });
            var container = new Container();
            var containerExtension = new StructureMapExtension(registrationHelper, options)
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
            var containerExtension = new StructureMapExtension(registrationHelper, options);

            //Act
            containerExtension.Run();

            //Assert
            Assert.AreSame(Mapper.Configuration, containerExtension.Resolve<IProfileExpression>());
            Assert.AreSame(Mapper.Engine, containerExtension.Resolve<IMappingEngine>());
            Assert.AreSame(containerExtension.Resolve<IProfileExpression>(), containerExtension.Resolve<IProfileExpression>());
            Assert.AreSame(containerExtension.Resolve<IMappingEngine>(), containerExtension.Resolve<IMappingEngine>());
        }
    }
}
