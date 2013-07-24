using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using AutoMapper;
using Bootstrap.Autofac;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions.Containers;
using Bootstrap.Extensions.StartupTasks;
using Bootstrap.Tests.Core.Extensions.StartupTasks;
using Bootstrap.Tests.Extensions.TestImplementations;
using Bootstrap.Tests.Other;
using CommonServiceLocator.AutofacAdapter.Unofficial;
using FakeItEasy;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Containers.Autofac
{
    [TestClass]
    public class AutofacExtensionTests
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
        public void ShouldCreateAnAutofacExtension()
        {
            //Act
            var result = new AutofacExtension(registrationHelper, options);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(AutofacExtension));
        }

        [TestMethod]
        public void ShouldAddAutofacToExcludedAssemblies()
        {
            //Act
            var result = new AutofacExtension(registrationHelper, options);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Bootstrapper.Excluding.Assemblies.Contains("Autofac"));
        }

        [TestMethod]
        public void ShouldReturnANullContainer()
        {
            //Arrange
            var containerExtension = new AutofacExtension(registrationHelper, options);

            //Act
            var result = containerExtension.Container;
            
            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldReturnAContainer()
        {
            //Arrange
            var containerExtension = new AutofacExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Container));
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfIBootstrapperRegistration()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(AutoMapperRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperRegistration>(assembly))
                .Returns(new List<Type> { typeof(AutoMapperRegistration) });
            var containerExtension = new AutofacExtension(registrationHelper, options);

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
        public void ShouldRegisterAllTypesOfIAutofacRegistration()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestAutofacRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IAutofacRegistration>(assembly))
                .Returns(new List<Type> { typeof(TestAutofacRegistration) });
            var containerExtension = new AutofacExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IAutofacRegistration>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<IAutofacRegistration>(assembly)).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IAutofacRegistration>));
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfModule()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestAutofacModule));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IModule>(assembly))
                .Returns(new List<Type> { typeof(TestAutofacModule) });
            var containerExtension = new AutofacExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IModule>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<IModule>(assembly)).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IModule>));
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIBootstrapperRegistrationTypes()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof (AutoMapperRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
               .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperRegistration>(assembly))
                .Returns(new List<Type> { typeof(AutoMapperRegistration) });
            var containerExtension = new AutofacExtension(registrationHelper, options);

            //Act
            containerExtension.Run();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperRegistration>(assembly)).MustHaveHappened();
            Assert.IsNotNull(containerExtension.Resolve<IProfileExpression>());
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllIAutofacRegistrationTypes()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestAutofacRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
               .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IAutofacRegistration>(assembly))
                .Returns(new List<Type> { typeof(TestAutofacRegistration) });
            var containerExtension = new AutofacExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.Resolve<AutofacExtension>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<IAutofacRegistration>(assembly)).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(AutofacExtension));
        }

        [TestMethod]
        public void ShouldInvokeTheLoadMethodOfAllIModuleTypes()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestAutofacModule));
            A.CallTo(() => registrationHelper.GetAssemblies())
               .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IModule>(assembly))
                .Returns(new List<Type> { typeof(TestAutofacModule) });
            var containerExtension = new AutofacExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.Resolve<ITestInterface>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<IModule>(assembly)).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ITestInterface));
        }


        [TestMethod]
        public void ShouldSetTheServiceLocator()
        {
            //Arrange
            ServiceLocator.SetLocatorProvider(() => null);
            var containerExtension = new AutofacExtension(registrationHelper, options);
            containerExtension.Run();

            //Act            
            containerExtension.SetServiceLocator();
            var result = ServiceLocator.Current;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(AutofacServiceLocator));
        }

        [TestMethod]
        public void ShouldResetTheServiceLocator()
        {
            //Arrange
            ServiceLocator.SetLocatorProvider(A.Fake<IServiceLocator>);
            var containerExtension = new AutofacExtension(registrationHelper, options);
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
            var containerExtension = new AutofacExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Container));
        }

        [TestMethod]
        public void ShouldResetTheContainer()
        {
            //Arrange
            var containerExtension = new AutofacExtension(registrationHelper, options);
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
            var containerExtension = new AutofacExtension(registrationHelper, options);
            var container = new ContainerBuilder().Build();

            //Act
            containerExtension.InitializeContainer(container);
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Container));
            Assert.AreSame(result, container);
        }

        [TestMethod]
        public void ShouldResolveASingleType()
        {
            //Arrange
            var containerExtension = new AutofacExtension(registrationHelper, options);
            var containerBuilder = new ContainerBuilder();
            var instance = new object();
            containerBuilder.RegisterInstance(instance);
            containerExtension.InitializeContainer(containerBuilder.Build());

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
            var containerExtension = new AutofacExtension(registrationHelper, options);
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<TaskAlpha>().As<IStartupTask>();
            containerBuilder.RegisterType<TaskBeta>().As<IStartupTask>();
            containerExtension.InitializeContainer(containerBuilder.Build());

            //Act
            var result = containerExtension.ResolveAll<IStartupTask>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(t => t is TaskAlpha));
            Assert.IsTrue(result.Any(t => t is TaskBeta));
        }

        [TestMethod]
        public void ShouldRegisterWithTargetAndImplementationType()
        {
            //Arrange
            var containerExtension = new AutofacExtension(registrationHelper, options);
            var containerBuilder = new ContainerBuilder();
            var container = containerBuilder.Build();
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.Register<IBootstrapperAssemblyProvider, LoadedAssemblyProvider>();
            var result = container.Resolve<IBootstrapperAssemblyProvider>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(LoadedAssemblyProvider));
        }

        [TestMethod]
        public void ShouldRegisterWithTargetAndImplementationInstance()
        {
            //Arrange
            var containerExtension = new AutofacExtension(registrationHelper, options);
            var containerBuilder = new ContainerBuilder();
            var container = containerBuilder.Build();
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.Register<IBootstrapperContainerExtension>(containerExtension);
            var result = container.Resolve<IBootstrapperContainerExtension>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(AutofacExtension));
            Assert.AreSame(containerExtension, result);
        }

        [TestMethod]
        public void ShouldRegisterWithTargetType()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestAutofacRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperAssemblyProvider>(assembly))
                .Returns(new List<Type> { typeof(LoadedAssemblyProvider), typeof(ReferencedAssemblyProvider) });
            var containerExtension = new AutofacExtension(registrationHelper, options);
            var containerBuilder = new ContainerBuilder();
            var container = containerBuilder.Build();
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.RegisterAll<IBootstrapperAssemblyProvider>();
            var result = container.Resolve<IEnumerable<IBootstrapperAssemblyProvider>>().ToList();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperAssemblyProvider>(assembly)).MustHaveHappened();
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IBootstrapperAssemblyProvider>));
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.Any(c => c is LoadedAssemblyProvider));
            Assert.IsTrue(result.Any(c => c is ReferencedAssemblyProvider));
        }

        [TestMethod]
        public void ShouldReturnAnAutofacOptions()
        {
            //Arrange
            var containerExtension = new AutofacExtension(registrationHelper, options);

            //Act
            var result = containerExtension.Options;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(AutofacOptions));
        }

        [TestMethod]
        public void ShouldRegisterWithConventionAndWithRegistration()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestAutofacRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IAutofacRegistration>(assembly))
                .Returns(new List<Type> { typeof(TestAutofacRegistration) });
            A.CallTo(() => options.AutoRegistration).Returns(true);
            var containerExtension = new AutofacExtension(registrationHelper, options);

            //Act
            containerExtension.Run();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<IAutofacRegistration>(assembly)).MustHaveHappened();
            Assert.IsNotNull(containerExtension.Resolve<IRegistrationHelper>());
            Assert.IsNotNull(containerExtension.Resolve<IRegisteredByConvention>());
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenSettingServiceLocatorBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new AutofacExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.SetServiceLocator);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenResolvingSimpleTypeBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new AutofacExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.Resolve<object>());

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenResolvingMultipleTypesBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new AutofacExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.ResolveAll<object>());

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenRegisteringWithTargetAndImplementationTypeBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new AutofacExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.Register<IBootstrapperContainerExtension, AutofacExtension>);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenRegisteringWithTargetAndImplementationInstanceBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new AutofacExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.Register<IBootstrapperContainerExtension>(containerExtension));

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenRegisteringWithTargetTypeBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new AutofacExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.RegisterAll<IBootstrapperContainerExtension>);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void Run_WhenTheContainerInOptionsIsSet_ShouldUseTheExistingContainer()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestAutofacRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IAutofacRegistration>(assembly))
                .Returns(new List<Type> { typeof(TestAutofacRegistration) });
            var container = new ContainerBuilder().Build();
            var containerExtension = new AutofacExtension(registrationHelper, options)
                {
                    Options = {Container = container}
                };


            //Act
            containerExtension.Run();

            //Assert
            Assert.AreSame(container, containerExtension.Container);            
        }
    }
}
