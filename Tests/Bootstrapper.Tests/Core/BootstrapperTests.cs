using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using Bootstrap.Autofac;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Bootstrap.Extensions.StartupTasks;
using Bootstrap.Locator;
using Bootstrap.Ninject;
using Bootstrap.StructureMap;
using Bootstrap.Tests.Core.Extensions.StartupTasks;
using Bootstrap.Tests.ExtensionForTest;
using Bootstrap.Unity;
using Bootstrap.Tests.Extensions.TestImplementations;
using Bootstrap.Windsor;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core
{
    [TestClass]
    public class BootstrapperTests
    {
        [TestInitialize]
        [TestCleanup]
        public void InitializeBootstrapper()
        {
            Bootstrapper.ClearExtensions();
        }

        [TestMethod]
        public void ShouldReturnANullContainer()
        {
            //Act
            var result = Bootstrapper.Container;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldReturnAnEmptyBootstrapperExtensions()
        {
            //Act
            var result = Bootstrapper.With;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BootstrapperExtensions));
            Assert.AreEqual(0, result.GetExtensions().Count);
        }

        [TestMethod]
        public void ShouldReturnANullContainerExtension()
        {
            var result = Bootstrapper.ContainerExtension;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldReturnTheSetContainer()
        {
            //Arrange
            var containerExtension = new TestContainerExtension(A.Fake<IRegistrationHelper>());
            Bootstrapper.With.Extension(containerExtension);

            //Act
            Bootstrapper.Start();
            var result = Bootstrapper.Container;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ShouldReturnABootstrapperExtensions()
        {
            //Arrange
            var containerExtension = A.Fake<IBootstrapperContainerExtension>();
            Bootstrapper.With.Extension(containerExtension);

            //Act
            var result = Bootstrapper.With;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BootstrapperExtensions));
            Assert.AreSame(containerExtension, result.GetExtensions()[0]);
            Assert.AreEqual(1, result.GetExtensions().Count);
        }
       
        [TestMethod]
        public void ShouldReturnTheContainerExtensionConfigured()
        {
            //Arrange
            var container = A.Fake<IBootstrapperContainerExtension>();
            Bootstrapper.With.Extension(container);

            //Act
            var result = Bootstrapper.ContainerExtension;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreSame(container, result);
        }

        [TestMethod]
        public void ShouldClearContainer()
        {
            //Arrange
            var containerExtension = new TestContainerExtension(A.Fake<IRegistrationHelper>());
            Bootstrapper.With.Extension(containerExtension);
            Bootstrapper.Start();

            //Act
            Bootstrapper.ClearExtensions();
            var result = Bootstrapper.Container;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldClearExtensions()
        {
            //Arrange
            var extension = A.Fake<IBootstrapperExtension>();
            Bootstrapper.With.Extension(extension);

            //Act
            Bootstrapper.ClearExtensions();
            var result =  Bootstrapper.GetExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(IList<IBootstrapperExtension>));
            Assert.AreEqual(0, Bootstrapper.GetExtensions().Count);
        }

        [TestMethod]
        public void ShouldNotHaveContainerExtension()
        {
            //Arrange
            var extension = A.Fake<IBootstrapperExtension>();
            Bootstrapper.With.Extension(extension);

            //Act
            Bootstrapper.ClearExtensions();
            var result = Bootstrapper.ContainerExtension;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldReturnAnEmptyExtensionIList()
        {
            var result = Bootstrapper.GetExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<IBootstrapperExtension>));
            Assert.AreEqual(0, Bootstrapper.GetExtensions().Count);
        }

        [TestMethod]
        public void ShouldReturnTheExtensionsAdded()
        {
            //Arrange
            var extension = A.Fake<IBootstrapperExtension>();
            Bootstrapper.With.Extension(extension);

            //Act
            var result = Bootstrapper.GetExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreSame(extension, result[0]);
        }

        [TestMethod]
        public void ShouldInvokeTheRunMethodOfAllTheRegisteredExtensions()
        {
            //Arrange
            var extension = A.Fake<IBootstrapperExtension>();
            Bootstrapper.With.Extension(extension);

            //Act
            Bootstrapper.Start();

            //Assert
            A.CallTo(() => extension.Run()).MustHaveHappened();
        }

        [TestMethod]
        public void ShouldInvokeTheResetMethodOfAllTheRegisteredExtensions()
        {
            //Arrange
            var extension = A.Fake<IBootstrapperExtension>();
            Bootstrapper.With.Extension(extension);

            //Act
            Bootstrapper.Reset();

            //Assert
            A.CallTo(() => extension.Reset()).MustHaveHappened();
        }

        [TestMethod]
        public void ShouldCompile()
        {
            //Act
            Bootstrapper
                .Including
                    .Assembly(Assembly.GetExecutingAssembly())
                    .AndAssembly(Assembly.GetAssembly(typeof(TestBootstrapperExtension)))
                .Excluding
                    .Assembly("StructureMap")
                    .AndAssembly("Microsoft.Practices")
                .IncludingOnly
                    .Assembly(Assembly.GetCallingAssembly())
                    .AndAssembly(Assembly.GetExecutingAssembly())
                    .AndAssembly(Assembly.GetAssembly(typeof(Mapper)))
                    .AndAssembly(Assembly.GetAssembly(typeof(AutoMapperExtension)))
                .With.Ninject().UsingAutoRegistration()
                .And.StructureMap().UsingAutoRegistration()
                .And.Unity().UsingAutoRegistration()
                .And.Windsor().UsingAutoRegistration()
                .And.Autofac().UsingAutoRegistration()
                .And.AutoMapper()
                .And.ServiceLocator()
                .And.StartupTasks()
                    .WithGroup(s => s
                        .First<TaskBeta>().DelayStartBy(5).MilliSeconds
                        .Then<TaskAlpha>())
                    .AndGroup(s => s
                        .First<TaskOmega>()
                        .Then().TheRest())
                .Excluding.Assembly("Castle");                
        }

        [TestMethod]
        public void ShouldReturnExcludedAssemblies()
        {
            //Act
            var result = Bootstrapper.Excluding.Assemblies;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<string>));
        }

        [TestMethod]
        public void ShouldResetExcludedAssemblies()
        {
            //Act
            Bootstrapper.Excluding.Assembly("Test");
            Bootstrapper.Reset();
            var result = Bootstrapper.Excluding.Assemblies;

            //Assert
            Assert.IsFalse(result.Contains("Test"));
        }

        [TestMethod]
        public void ShouldReturnIncludedAssemblies()
        {
            //Act
            var result = Bootstrapper.Including.Assemblies;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Assembly>));
        }

        [TestMethod]
        public void ShouldResetIncludedAssemblies()
        {
            //Act
            Bootstrapper.Including.Assembly(Assembly.GetExecutingAssembly());
            Bootstrapper.Reset();
            var result = Bootstrapper.Including.Assemblies;

            //Assert
            Assert.IsFalse(result.Contains(Assembly.GetExecutingAssembly()));
        }

        [TestMethod]
        public void ShouldReturnIncludedOnlyAssemblies()
        {
            //Act
            var result = Bootstrapper.IncludingOnly.Assemblies;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Assembly>));
        }

        [TestMethod]
        public void ShouldResetIncludedOnlyAssemblies()
        {
            //Act
            Bootstrapper.IncludingOnly.Assembly(Assembly.GetExecutingAssembly());
            Bootstrapper.Reset();
            var result = Bootstrapper.IncludingOnly.Assemblies;

            //Assert
            Assert.IsFalse(result.Contains(Assembly.GetExecutingAssembly()));
        }
    }
}
