using System.Collections.Generic;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Bootstrap.StartupTasks;
using Bootstrap.Tests.Extensions.TestImplementations;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var containerExtension = A.Fake<IBootstrapperContainerExtension>();
            var task = A.Fake<IStartupTask>();
            A.CallTo(() => containerExtension.ResolveAll<IStartupTask>()).Returns(new List<IStartupTask> {task});
            Bootstrapper.With.Extension(containerExtension).And.Extension(new StartupTasksExtension());

            //Act
            Bootstrapper.Start();

            //Assert
            A.CallTo(() => task.Run()).MustHaveHappened();
        }

        [TestMethod]
        public void ShouldExecuteTheResetMethodOfAllStartupTasks()
        {
            //Arrange
            var containerExtension = A.Fake<IBootstrapperContainerExtension>();
            var task = A.Fake<IStartupTask>();
            A.CallTo(() => containerExtension.ResolveAll<IStartupTask>()).Returns(new List<IStartupTask> { task });
            Bootstrapper.With.Extension(containerExtension).And.Extension(new StartupTasksExtension());

            //Act
            Bootstrapper.Reset();

            //Assert
            A.CallTo(() => task.Reset()).MustHaveHappened();
        }

        [TestMethod]
        public void ShouldExecuteTheRunMethodForAllStartupTasksWhenNoContainerExtensionHasBeenDeclared()
        {
            //Arrange
            var tasksExtension = new StartupTasksExtension();

            //Act
            tasksExtension.Run();

            //Assert
            Assert.IsTrue(TestStartupTask.Invoked);
        }

        [TestMethod]
        public void ShouldExecuteTheResetMethodForAllStartupTasksWhenNoContainerExtensionHasBeenDeclared()
        {
            //Arrange
            var tasksExtension = new StartupTasksExtension();

            //Act
            tasksExtension.Run();
            tasksExtension.Reset();

            //Assert
            Assert.IsFalse (TestStartupTask.Invoked);
        }
    }
}
