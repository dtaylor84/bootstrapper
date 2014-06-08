using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions.Containers;
using Bootstrap.Extensions.StartupTasks;
using Bootstrap.Ninject;
using Bootstrap.Tests.Core.Extensions.StartupTasks;
using Bootstrap.Tests.Extensions.TestImplementations;
using Bootstrap.Tests.Other;
using CommonServiceLocator.NinjectAdapter.Unofficial;
using FakeItEasy;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Modules;
using Shouldly;

namespace Bootstrap.Tests.Extensions.Containers.Ninject
{
    [TestClass]
    public class NinjectExtensionTests
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
        public void ShouldCreateANinjectExtension()
        {
            //Act
            var result = new NinjectExtension(registrationHelper, options);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NinjectExtension));
        }

        [TestMethod]
        public void ShouldAddNinjectToExcludedAssemblies()
        {
            //Act
            var result = new NinjectExtension(registrationHelper, options);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Bootstrapper.Excluding.Assemblies.Contains("Ninject"));
        }

        [TestMethod]
        public void ShouldReturnANullContainer()
        {
            //Arrange
            var containerExtension = new NinjectExtension(registrationHelper, options);

            //Act
            var result = containerExtension.Container;
            
            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldReturnAnIKernel()
        {
            //Arrange
            var containerExtension = new NinjectExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IKernel));
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
            var containerExtension = new NinjectExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<IBootstrapperRegistration>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperRegistration>(assembly)).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IBootstrapperRegistration>));
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result[0] is AutoMapperRegistration);
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfINinjectRegistration()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestNinjectRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<INinjectRegistration>(assembly))
                .Returns(new List<Type> { typeof(TestNinjectRegistration) });
            var containerExtension = new NinjectExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<INinjectRegistration>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<INinjectRegistration>(assembly)).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<INinjectRegistration>));
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void ShouldRegisterAllTypesOfINinjectModule()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestNinjectModule));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<INinjectModule>(assembly))
                .Returns(new List<Type> { typeof(TestNinjectModule) });
            var containerExtension = new NinjectExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.ResolveAll<INinjectModule>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<INinjectModule>(assembly)).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<INinjectModule>));
            Assert.IsTrue(result.Any());
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
            var containerExtension = new NinjectExtension(registrationHelper, options);

            //Act
            containerExtension.Run();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperRegistration>(assembly)).MustHaveHappened();
            Assert.IsNotNull(containerExtension.Resolve<IProfileExpression>());
        }

        [TestMethod]
        public void ShouldInvokeTheRegisterMethodOfAllINinjectRegistrationTypes()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestNinjectRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<INinjectRegistration>(assembly))
                .Returns(new List<Type> { typeof(TestNinjectRegistration) });
            var containerExtension = new NinjectExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.Resolve<NinjectExtension>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<INinjectRegistration>(assembly)).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NinjectExtension));
        }

        [TestMethod]
        public void ShouldInvokeTheLoadMethodOfAllINinjectModuleTypes()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestNinjectModule));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<INinjectModule>(assembly))
                .Returns(new List<Type> { typeof(TestNinjectModule) });
            var containerExtension = new NinjectExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.Resolve<ITestInterface>();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<INinjectModule>(assembly)).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ITestInterface));
        }

        [TestMethod]
        public void ShouldSetTheServiceLocator()
        {
            //Arrange
            ServiceLocator.SetLocatorProvider(() => null);
            var containerExtension = new NinjectExtension(registrationHelper, options);
            containerExtension.Run();

            //Act            
            containerExtension.SetServiceLocator();
            var result = ServiceLocator.Current;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NinjectServiceLocator));
        }

        [TestMethod]
        public void ShouldResetTheServiceLocator()
        {
            //Arrange
            ServiceLocator.SetLocatorProvider(A.Fake<IServiceLocator>);
            var containerExtension = new NinjectExtension(registrationHelper, options);
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
            var containerExtension = new NinjectExtension(registrationHelper, options);

            //Act
            containerExtension.Run();
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IKernel));
        }

        [TestMethod]
        public void ShouldResetTheContainer()
        {
            //Arrange
            var containerExtension = new NinjectExtension(registrationHelper, options);
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
            var containerExtension = new NinjectExtension(registrationHelper, options);
            var container = A.Fake<IKernel>();

            //Act
            containerExtension.InitializeContainer(container);
            var result = containerExtension.Container;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IKernel));
            Assert.AreSame(result, container);
        }

        [TestMethod]
        public void ShouldResolveASingleType()
        {
            //Arrange
            var containerExtension = new NinjectExtension(registrationHelper, options);
            var container = new StandardKernel();
            var instance = new object();
            container.Bind<object>().ToConstant(instance);
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
            var containerExtension = new NinjectExtension(registrationHelper, options);
            var container = new StandardKernel();
            container.Bind<IStartupTask>().To<TaskAlpha>();
            container.Bind<IStartupTask>().To<TaskBeta>();
            containerExtension.InitializeContainer(container);

            //Act
            var result = containerExtension.ResolveAll<IStartupTask>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(t => t is TaskAlpha));
            Assert.IsTrue(result.Any(t => t is TaskBeta));
        }

        [TestMethod]
        public void RegisterAll_WhenInvokedWithNonGenericTargetType_ShouldRegisterType()
        {
            //Arrange
            var container = new StandardKernel();
            var containerExtension = new NinjectExtension(registrationHelper, options);
            containerExtension.InitializeContainer(container);
            var thisAssembly = Assembly.GetCallingAssembly();
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new[] { thisAssembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing(thisAssembly, typeof(IGenericTest<>)))
                .Returns(new[] { typeof(GenericTest<>) });

            //Act
            containerExtension.RegisterAll(typeof(IGenericTest<>));
            var result1 = container.Get<IGenericTest<object>>();
            var result2 = container.Get<IGenericTest<string[]>>();

            //Assert
            result1.ShouldBeOfType<GenericTest<object>>();
            result2.ShouldBeOfType<GenericTest<string[]>>();
        }

        [TestMethod]
        public void ShouldRegisterWithTargetAndImplementationType()
        {
            //Arrange
            var container = new StandardKernel();
            var containerExtension = new NinjectExtension(registrationHelper, options);
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.Register<IBootstrapperAssemblyProvider, LoadedAssemblyProvider>();
            var result = container.Get<IBootstrapperAssemblyProvider>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(LoadedAssemblyProvider));
        }

        [TestMethod]
        public void ShouldRegisterWithTargetAndImplementationInstance()
        {
            //Arrange
            var container = new StandardKernel();
            var containerExtension = new NinjectExtension(registrationHelper, options);
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.Register<IBootstrapperContainerExtension>(containerExtension);
            var result = container.Get<IBootstrapperContainerExtension>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NinjectExtension));
            Assert.AreSame(containerExtension, result);
        }

        [TestMethod]
        public void ShouldRegisterWithTargetType()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(RegistrationHelper));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperAssemblyProvider>(assembly))
                .Returns(new List<Type> { typeof(LoadedAssemblyProvider), typeof(ReferencedAssemblyProvider) });
            var container = new StandardKernel();
            var containerExtension = new NinjectExtension(registrationHelper, options);
            containerExtension.InitializeContainer(container);

            //Act
            containerExtension.RegisterAll<IBootstrapperAssemblyProvider>();
            var result = container.GetAll<IBootstrapperAssemblyProvider>().ToList();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<IBootstrapperAssemblyProvider>(assembly)).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IBootstrapperAssemblyProvider>));
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.Any(c => c is LoadedAssemblyProvider));
            Assert.IsTrue(result.Any(c => c is ReferencedAssemblyProvider));
        }

        [TestMethod]
        public void ShouldReturnANinjectOptions()
        {
            //Arrange
            var containerExtension = new NinjectExtension(registrationHelper, options);

            //Act
            var result = containerExtension.Options;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(NinjectOptions));
        }

        [TestMethod]
        public void ShouldRegisterWithConventionAndWithRegistration()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestNinjectRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<INinjectRegistration>(assembly))
                .Returns(new List<Type> { typeof(TestNinjectRegistration) });
            var containerExtension = new NinjectExtension(registrationHelper, options);
            A.CallTo(() => options.AutoRegistration).Returns(true);

            //Act
            containerExtension.Run();

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            A.CallTo(() => registrationHelper.GetTypesImplementing<INinjectRegistration>(assembly)).MustHaveHappened();
            Assert.IsNotNull(containerExtension.Resolve<IRegistrationHelper>());
            Assert.IsNotNull(containerExtension.Resolve<NinjectExtension>());
            Assert.IsNotNull(containerExtension.Resolve<IRegisteredByConvention>());
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenSettingServiceLocatorBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new NinjectExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.SetServiceLocator);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenResolvingSimpleTypeBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new NinjectExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.Resolve<object>());

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenResolvingMultipleTypesBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new NinjectExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.ResolveAll<object>());

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void Register_WhenInvokedWithNonGenericTargetAndContainerIsNotInitialized_ShouldThrowException()
        {
            //Arrange
            var containerExtension = new NinjectExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.RegisterAll(typeof(IGenericTest<>)));

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenRegisteringWithTargetAndImplementationTypeBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new NinjectExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.Register<IBootstrapperContainerExtension, NinjectExtension>);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenRegisteringWithTargetAndImplementationInstanceBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new NinjectExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(() => containerExtension.Register<IBootstrapperContainerExtension>(containerExtension));

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldThrowNoContainerExceptionWhenRegisteringWithTargetTypeBeforeInitializingTheContainer()
        {
            //Arrange
            var containerExtension = new NinjectExtension(registrationHelper, options);

            //Act
            var result = ExceptionAssert.Throws<NoContainerException>(containerExtension.RegisterAll<IBootstrapperContainerExtension>);

            //Assert
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void Run_WhenTheContainerInOptionsIsSet_ShouldUseTheExistingContainer()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(TestNinjectRegistration));
            A.CallTo(() => registrationHelper.GetAssemblies())
                .Returns(new List<Assembly> { assembly });
            A.CallTo(() => registrationHelper.GetTypesImplementing<INinjectRegistration>(assembly))
                .Returns(new List<Type> { typeof(TestNinjectRegistration) });
            var container = new StandardKernel();
            var containerExtension = new NinjectExtension(registrationHelper, options)
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
            var containerExtension = new NinjectExtension(registrationHelper, options);

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
