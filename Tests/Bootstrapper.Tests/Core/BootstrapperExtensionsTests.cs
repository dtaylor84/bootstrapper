using System;
using System.Collections.Generic;
using System.Reflection;
using Bootstrap.Tests.Other;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bootstrap.Tests.Core
{
    [TestClass]
    public class BootstrapperExtensionsTests
    {
        [TestMethod]
        public void ShouldCreateABoostrapperExtensions()
        {
            //Act
            var result = new BootstrapperExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BootstrapperExtensions));
        }

        [TestMethod]
        public void ShouldReturnTheSameInstance()
        {
            //Arrange
            var extensions = new BootstrapperExtensions();

            //Act
            var result = extensions.With;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(BootstrapperExtensions));
            Assert.AreSame(extensions,result);
        }

        [TestMethod]
        public void ShouldHaveNoExtensions()
        {
            //Arrange
            var extensions = new BootstrapperExtensions();

            //Act
            extensions.ClearExtensions();
            var result = extensions.GetExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(IList<IBootstrapperExtension>));
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldAddAnExtension()
        {
            //Arrange
            var extension = new Mock<BootstrapperExtension>();
            var extensions = new BootstrapperExtensions();

            //Act
            extensions.Extension(extension.Object);
            var result = extensions.GetExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(IList<IBootstrapperExtension>));
            Assert.AreEqual(1, result.Count);
            Assert.AreSame(extension.Object, result[0]);
        }

        [TestMethod]
        public void ShouldSetTheContainer()
        {
            //Arrange
            var container = new Mock<IBootstrapperContainerExtension>();
            var extensions = new BootstrapperExtensions();

            //Act
            extensions.Container(container.Object);
            var result = extensions.GetContainerExtension();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtension));
            Assert.AreSame(container.Object, result);
        }

        [TestMethod]
        public void ShouldReturnAReadOnlyListOfBoostraperExtensions()
        {
            //Arrange
            var extension = new Mock<BootstrapperExtension>();
            var newExtension = new Mock<BootstrapperExtension>();
            var extensions = new BootstrapperExtensions();
            extensions.Extension(extension.Object);

            //Act
            var result = extensions.GetExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<IBootstrapperExtension>));
            Assert.AreEqual(1, result.Count);
            ExceptionAssert.Throws<NotSupportedException>(() => result.Add(newExtension.Object));
        }

        [TestMethod]
        public void ShouldInvokeTheStartMethodOftheBootStrapper()
        {
            //Arrange
            var extension = new Mock<IBootstrapperExtension>();
            Bootstrap.Bootstrapper.With.Extension(extension.Object);

            //Act
            Bootstrap.Bootstrapper.With.Start();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            extension.Verify(e => e.Run(), Times.Once());
        }

        [TestMethod]
        public void ShouldCaptureTheStartCallingAssembly()
        {
            //Act
            Bootstrap.Bootstrapper.With.Start();
            var result = Bootstrap.Bootstrapper.GetStartCallingAssembly();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Assembly));
            Assert.AreSame(result, Assembly.GetExecutingAssembly());
        }

    }
}
