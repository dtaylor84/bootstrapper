﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions.Containers;
using Bootstrap.StructureMap;
using Bootstrap.Tests.Extensions.TestImplementations;
using Bootstrap.Tests.Other;
using FakeItEasy;
using StructureMap;
using StructureMap.Configuration.DSL;
using CommonServiceLocator.StructureMapAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceLocation;
using System.Reflection;

namespace Bootstrap.Tests.Extensions.Containers.StructureMap
{
    [TestClass]
    public class StructureMapExtensionTests
    {
        private IRegistrationHelper registrationHelper;

        [TestInitialize]
        public void InitializeBootstrapper()
        {
            Bootstrapper.ClearExtensions();
            registrationHelper = A.Fake<IRegistrationHelper>();
        }

        [TestMethod]
        public void ShouldCreateAStructureMapExtension()
        {
            //Act
            var result = new StructureMapExtension(registrationHelper);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StructureMapExtension));
        }

        [TestMethod]
        public void ShouldAddStructureMapToExcludedAssemblies()
        {
            //Act
            var result = new StructureMapExtension(registrationHelper);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Bootstrapper.Excluding.Assemblies.Contains("StructureMap"));
        }

        [TestMethod]
        public void ShouldReturnANullContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension(registrationHelper);

            //Act
            var result = containerExtension.Container;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldReturnAnIContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension(registrationHelper);

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
            var containerExtension = new StructureMapExtension(registrationHelper);

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
            var containerExtension = new StructureMapExtension(registrationHelper);

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
            var containerExtension = new StructureMapExtension(registrationHelper);

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
            var containerExtension = new StructureMapExtension(registrationHelper);

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
            var containerExtension = new StructureMapExtension(registrationHelper);

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
            var containerExtension = new StructureMapExtension(registrationHelper);

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
            var containerExtension = new StructureMapExtension(registrationHelper);
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
            var containerExtension = new StructureMapExtension(registrationHelper);
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
            var containerExtension = new StructureMapExtension(registrationHelper);

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
            var containerExtension = new StructureMapExtension(registrationHelper);
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
            var containerExtension = new StructureMapExtension(registrationHelper);
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
            var containerExtension = new StructureMapExtension(registrationHelper);
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
            var containerExtension = new StructureMapExtension(registrationHelper);
            var container = A.Fake<IContainer>();
            containerExtension.InitializeContainer(container);
            var instances = new List<object> { new object(), new object() };
            A.CallTo(() => container.GetAllInstances<object>()).Returns(instances);
            
            //Act
            var result = containerExtension.ResolveAll<object>();

            //Assert
            A.CallTo(() => container.GetAllInstances<object>()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.AreSame(instances, result);
        }

        [TestMethod]
        public void ShouldRegisterWithTargetAndImplementationType()
        {
            //Arrange
            var container = new Container();
            var containerExtension = new StructureMapExtension(registrationHelper);
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.Register<IRegistrationHelper, RegistrationHelper>();
            var result = container.GetInstance<IRegistrationHelper>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RegistrationHelper));
        }

        [TestMethod]
        public void ShouldRegisterWithTargetAndImplementationInstance()
        {
            //Arrange
            var container = new Container();
            var containerExtension = new StructureMapExtension(registrationHelper);
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
            var container = new Container();
            var containerExtension = new StructureMapExtension(registrationHelper);
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.RegisterAll<IRegistrationHelper>();
            var result = container.GetAllInstances<IRegistrationHelper>();

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
            var containerExtension = new StructureMapExtension(registrationHelper);

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
            var assembly = Assembly.GetAssembly(typeof(StructureMapExtensionTests));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });

            var containerExtension = new StructureMapExtension(registrationHelper);
            containerExtension.Options.UsingAutoRegistration();

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
            var containerExtension = new StructureMapExtension(registrationHelper);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.SetServiceLocator);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenResolvingSimpleTypeBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension(registrationHelper);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.Resolve<object>());

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod] public void ShouldThrowNoContainerExceptionWhenResolvingMultipleTypesBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension(registrationHelper);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.ResolveAll<object>());

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenRegisteringWithTargetAndImplementationTypeBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension(registrationHelper);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.Register<IBootstrapperContainerExtension, StructureMapExtension>);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenRegisteringWithTargetAndImplementationInstanceBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension(registrationHelper);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.Register<IBootstrapperContainerExtension>(containerExtension));

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenRegisteringWithTargetTypeBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new StructureMapExtension(registrationHelper);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.RegisterAll<IBootstrapperContainerExtension>);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }
    }
}
