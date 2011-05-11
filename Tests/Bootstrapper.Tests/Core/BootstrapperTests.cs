using System.Collections.Generic;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Bootstrap.ServiceLocator;
using Bootstrap.StartupTasks;
using Bootstrap.StructureMap;
using Bootstrap.Tests.Extensions.TestImplementations;
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
            var containerExtension = new TestContainerExtension();
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
            var containerExtension = new TestContainerExtension();
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
            Bootstrapper.With.StructureMap().And.AutoMapper().And.ServiceLocator().And.StartupTasks().Start();
        }


    }
}
