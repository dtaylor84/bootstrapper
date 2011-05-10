using System.Collections.Generic;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Bootstrap.ServiceLocator;
using Bootstrap.StartupTasks;
using Bootstrap.StructureMap;
using Bootstrap.Tests.Extensions.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
        public void ShouldReturnAnEmptyExtensionIList()
        {
            var result = Bootstrapper.GetExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<IBootstrapperExtension>));
            Assert.AreEqual(0, Bootstrapper.GetExtensions().Count);
        }


        [TestMethod]
        public void ShouldNotHaveExtensions()
        {
            //Arrange
            var extension = new Mock<IBootstrapperExtension>();
            Bootstrapper.With.Extension(extension.Object);

            //Act
            Bootstrapper.ClearExtensions();
            var result =  Bootstrapper.GetExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(IList<IBootstrapperExtension>));
            Assert.AreEqual(0, Bootstrapper.GetExtensions().Count);
        }

        [TestMethod]
        public void ShouldReturnANullContainerExtension()
        {
            var result = Bootstrapper.ContainerExtension;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldNotHaveContainerExtension()
        {
            //Arrange
            var extension = new Mock<IBootstrapperExtension>();
            Bootstrapper.With.Extension(extension.Object);

            //Act
            Bootstrapper.ClearExtensions();
            var result = Bootstrapper.ContainerExtension;

            //Assert
            Assert.IsNull(result);
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
        public void ShouldNotHaveContainer()
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
            //Act
            var result = Bootstrapper.With;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BootstrapperExtensions));
        }

        [TestMethod]
        public void ShouldReturnTheExtensionsAdded()
        {
            //Arrange
            var extension = new Mock<IBootstrapperExtension>();
            Bootstrapper.With.Extension(extension.Object);

            //Act
            var result = Bootstrapper.GetExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreSame(extension.Object, result[0]);
        }

        [TestMethod]
        public void ShouldReturnTheContainerConfigured()
        {
            //Arrange
            var container = new Mock<IBootstrapperContainerExtension>();
            Bootstrapper.With.Extension(container.Object);

            //Act
            var result = Bootstrapper.ContainerExtension;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreSame(container.Object, result);
        }

        [TestMethod]
        public void ShouldInvokeTheResetMethodOfAllTheRegisteredExtensions()
        {
            //Arrange
            var extension = new Mock<IBootstrapperExtension>();
            Bootstrapper.With.Extension(extension.Object);

            //Act
            Bootstrapper.Reset();

            //Assert
            extension.Verify(e => e.Reset(), Times.Once());
        }

        [TestMethod]
        public void ShouldInvokeTheRunMethodOfAllTheRegisteredExtensions()
        {
            //Arrange
            var extension = new Mock<IBootstrapperExtension>();
            Bootstrapper.With.Extension(extension.Object);

            //Act
            Bootstrapper.Start();

            //Assert
            extension.Verify(e => e.Run(), Times.Once());
        }

        [TestMethod]
        public void ShouldCompile()
        {
            //Act
            Bootstrapper.With.StructureMap().And.AutoMapper().And.ServiceLocator().And.StartupTasks().Start();
        }


    }
}
