using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions.Containers;
using Bootstrap.Tests.Extensions.TestImplementations;
using Bootstrap.Tests.Other;
using Bootstrap.Unity;
using FakeItEasy;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Containers.Unity
{
    [TestClass]
    public class UnityExtensionTests
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
        public void ShouldCreateAUnityExtension()
        {
            //Act
            var result = new UnityExtension(registrationHelper, options);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnityExtension));
        }

        [TestMethod]
        public void ShouldAddMicrosoftPracticesToExcludedAssemblies()
        {
            //Act
            var result = new UnityExtension(registrationHelper, options);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Bootstrapper.Excluding.Assemblies.Contains("Microsoft.Practices"));
        }

        [TestMethod]
        public void ShouldReturnANullContainer()
        {
            //Arrange
            var containerExtension = new UnityExtension(registrationHelper, options);

            //Act
            var result = containerExtension.Container;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldReturnAnIUnityContainer()
        {
            //Arrange
            var containerExtension = new UnityExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IUnityContainer));
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfIBootstrapperRegistration()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof (AutoMapperRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> {assembly});
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperRegistration>(assembly))
                .Returns(new List<Type> {typeof (AutoMapperRegistration)});
            var containerExtension = new UnityExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IBootstrapperRegistration>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperRegistration>(assembly)).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IBootstrapperRegistration>));
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result[0] is AutoMapperRegistration);
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfIUnityRegistration()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestUnityRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IUnityRegistration>(assembly))
                .Returns(new List<Type> { typeof(TestUnityRegistration) }); 
            var containerExtension = new UnityExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IUnityRegistration>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<IUnityRegistration>(assembly)).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IUnityRegistration>));
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIBootstrapperRegistrationTypes()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(AutoMapperRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperRegistration>(assembly))
                .Returns(new List<Type> { typeof(AutoMapperRegistration) });
            var containerExtension = new UnityExtension(registrationHelper, options);

            //Act
            containerExtension.Run();          

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperRegistration>(assembly)).MustHaveHappened();
            Assert.IsNotNull(containerExtension.Resolve<IProfileExpression>());
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIUnityRegistrationTypes()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestUnityRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IUnityRegistration>(assembly))
                .Returns(new List<Type> { typeof(TestUnityRegistration) }); 
            var containerExtension = new UnityExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.Resolve<UnityExtension>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<IUnityRegistration>(assembly)).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnityExtension));
        }

        [TestMethod]
        public void ShouldSetTheServiceLocator()
        {
            //Arrange
            ServiceLocator.SetLocatorProvider(() => null);
            var containerExtension = new UnityExtension(registrationHelper, options);
            containerExtension.Run();

            //Act
            containerExtension.SetServiceLocator();
            var result = ServiceLocator.Current;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnityServiceLocator));
        }

        [TestMethod]
        public void ShouldResetTheServiceLocator()
        {
            //Arrange
            ServiceLocator.SetLocatorProvider(A.Fake<IServiceLocator>);
            var containerExtension = new UnityExtension(registrationHelper, options);
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
            var containerExtension = new UnityExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IUnityContainer));
        }

        [TestMethod]
        public void ShouldResetTheContainer()
        {
            //Arrange
            var containerExtension = new UnityExtension(registrationHelper, options);
            containerExtension.Run();

            //Act
            containerExtension.Reset();

            //Assert
            Assert.IsNull(containerExtension.Container);
        }


        public void ShouldInitializeTheContainerToTheValuePassed()
        {
            //Arrange
            var containerExtension = new UnityExtension(registrationHelper, options);
            var container = A.Fake<IUnityContainer>();

            //Act
            containerExtension.InitializeContainer(container);
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IUnityContainer));
            Assert.AreSame(result, container);
        }

        [TestMethod]
        public void ShouldResolveASingleUnnamedType()
        {
            //Arrange
            var containerExtension = new UnityExtension(registrationHelper, options);
            var container = new UnityContainer();
            var instance1 = new object();
            var instance2 = new object();
            container.RegisterInstance(instance1);
            container.RegisterInstance("Name", instance2);
            containerExtension.InitializeContainer(container);

            //Act
            var result = containerExtension.Resolve<object>();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreSame(instance1, result);
        }

        [TestMethod]
        public void ShouldResolveASingleNamedType()
        {
            //Arrange
            var containerExtension = new UnityExtension(registrationHelper, options);
            var container = new UnityContainer();
            var instance = new object();
            container.RegisterInstance("Name", instance);
            containerExtension.InitializeContainer(container);

            //Act
            var result = containerExtension.Resolve<object>();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreSame(instance, result);
        }

        [TestMethod]
        public void ShouldResolveMultipleTypes()
        {
            //Arrange
            var containerExtension = new UnityExtension(registrationHelper, options);
            var container = new UnityContainer();
            var instance1 = new object();
            var instance2 = new object();
            container.RegisterInstance("Name1", instance1);
            container.RegisterInstance("Name2", instance2);
            containerExtension.InitializeContainer(container);

            //Act
            var result = containerExtension.ResolveAll<object>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(o => o == instance1));
            Assert.IsTrue(result.Any(o => o == instance2));
        }

        [TestMethod]
        public void ShouldRegisterWithTargetAndImplementationType()
        {
            //Arrange
            var container = new UnityContainer();
            var containerExtension = new UnityExtension(registrationHelper, options);
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.Register<IRegistrationHelper, RegistrationHelper>();
            var result = container.Resolve<IRegistrationHelper>(typeof(RegistrationHelper).Name);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RegistrationHelper));
        }

        [TestMethod]
        public void ShouldRegisterWithTargetAndImplementationInstance()
        {
            //Arrange
            var container = new UnityContainer();
            var containerExtension = new UnityExtension(registrationHelper, options);
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.Register<IBootstrapperContainerExtension>(containerExtension);
            var result = container.Resolve<IBootstrapperContainerExtension>(typeof(UnityExtension).Name);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnityExtension));
            Assert.AreSame(containerExtension, result);
        }

        [TestMethod]
        public void ShouldRegisterWithTargetType()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(RegistrationHelper));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IRegistrationHelper>(assembly))
                .Returns(new List<Type> { typeof(RegistrationHelper) });
            var container = new UnityContainer();
            var containerExtension = new UnityExtension(registrationHelper, options);
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.RegisterAll<IRegistrationHelper>();
            var result = container.ResolveAll<IRegistrationHelper>().ToList();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<IRegistrationHelper>(assembly)).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IRegistrationHelper>));
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.Any(c => c is RegistrationHelper));
        }

        [TestMethod]
        public void ShouldReturnAUnityOptions()
        {
            //Arrange
            var containerExtension = new UnityExtension(registrationHelper, options);

            //Act
            var result = containerExtension.Options;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(UnityOptions));
        }

        [TestMethod]
        public void ShouldRegisterWithConventionAndWithRegistration()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestUnityRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IUnityRegistration>(assembly))
                .Returns(new List<Type> { typeof(TestUnityRegistration) });
            var containerExtension = new UnityExtension(registrationHelper, options);
            A.CallTo(() => options.AutoRegistration).Returns(true);

            //Act
            containerExtension.Run();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<IUnityRegistration>(assembly)).MustHaveHappened();
            Assert.IsNotNull(containerExtension.Resolve<IRegistrationHelper>());
            Assert.IsNotNull(containerExtension.Resolve<UnityExtension>());
            Assert.IsNotNull(containerExtension.Resolve<IRegisteredByConvention>());
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenSettingServiceLocatorBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new UnityExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.SetServiceLocator);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenResolvingSimpleTypeBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new UnityExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.Resolve<object>());

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenResolvingMultipleTypesBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new UnityExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.ResolveAll<object>());

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenRegisteringWithTargetAndImplementationTypeBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new UnityExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.Register<IBootstrapperContainerExtension, UnityExtension>);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenRegisteringWithTargetAndImplementationInstanceBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new UnityExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.Register<IBootstrapperContainerExtension>(containerExtension));

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenRegisteringWithTargetTypeBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new UnityExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.RegisterAll<IBootstrapperContainerExtension>);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void Run_WhenTheContainerInOptionsIsSet_ShouldUseTheExistingContainer()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestUnityRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IUnityRegistration>(assembly))
                .Returns(new List<Type> { typeof(TestUnityRegistration) });
            var container = new UnityContainer();
            var containerExtension = new UnityExtension(registrationHelper, options)
            {
                Options = { Container = container }
            };


            //Act
            containerExtension.Run();

            //Assert
            Assert.AreSame(container, containerExtension.Container);
        }
    }
}
