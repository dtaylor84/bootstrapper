﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions.Containers;
using Bootstrap.SimpleInjector;
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
    }
}
