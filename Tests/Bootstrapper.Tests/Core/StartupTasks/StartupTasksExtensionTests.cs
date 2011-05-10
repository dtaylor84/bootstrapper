using System.Collections.Generic;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Bootstrap.StartupTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bootstrap.Tests.Core.StartupTasks
{
    [TestClass]
    public class StartupTasksExtensionTests
    {
        [TestInitialize]
        public void Initialize()
        {
            Bootstrapper.ClearExtensions();
        }

        [TestMethod]
        public void ShouldCreateANewStartupTasksExtension()
        {
            //Act
            var result = new StartupTasksExtension();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperExtension));
            Assert.IsInstanceOfType(result, typeof(StartupTasksExtension));
        }

        [TestMethod]
        public void ShouldExecuteTheRunMethodOfAllStartupTasks()
        {
            //Arrange
            var containerExtension = new Mock<IBootstrapperContainerExtension>();
            var task = new Mock<IStartupTask>();
            containerExtension.Setup(c => c.ResolveAll<IStartupTask>()).Returns(new List<IStartupTask> { task.Object });
            Bootstrapper.With.Extension(containerExtension.Object).And.Extension(new StartupTasksExtension());

            //Act
            Bootstrapper.Start();

            //Assert
            task.Verify(t => t.Run(), Times.Once());
        }

        [TestMethod]
        public void ShouldExecuteTheResetMethodOfAllStartupTasks()
        {
            //Arrange
            var containerExtension = new Mock<IBootstrapperContainerExtension>();
            var task = new Mock<IStartupTask>();
            containerExtension.Setup(c => c.ResolveAll<IStartupTask>()).Returns(new List<IStartupTask> { task.Object });
            Bootstrapper.With.Extension(containerExtension.Object).And.Extension(new StartupTasksExtension());

            //Act
            Bootstrapper.Reset();

            //Assert
            task.Verify(t => t.Reset(), Times.Once());
        }

    }
}
