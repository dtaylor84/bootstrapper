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

        [TestMethod]
        public void ShouldReturnAnEmptyExecutionLog()
        {
            var taskExtension = new StartupTasksExtension();

            //Act
            var result = taskExtension.ExecutionLog;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<string>));
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldLogTheExecutionOrderOfTasks()
        {
            var tasksExtension = new StartupTasksExtension();

            //Act
            tasksExtension.Run();
            var result = tasksExtension.ExecutionLog;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<string>));
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.Contains("+TaskOmega"));
            Assert.IsTrue(result.Contains("+TaskAlpha"));
            Assert.IsTrue(result.Contains("+TaskBeta"));
            Assert.IsTrue(result.Contains("+TestStartupTask"));
        }

        [TestMethod]
        public void ShouldLogTheResetOrderOfTasks()
        {
            var tasksExtension = new StartupTasksExtension();

            //Act
            tasksExtension.Reset();
            var result = tasksExtension.ExecutionLog;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<string>));
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.Contains("-TaskOmega"));
            Assert.IsTrue(result.Contains("-TaskAlpha"));
            Assert.IsTrue(result.Contains("-TaskBeta"));
            Assert.IsTrue(result.Contains("-TestStartupTask"));
        }

        [TestMethod]
        public void ShouldExecuteTasksInSequenceUsingTheTaskAttribute()
        {
            var tasksExtension = new StartupTasksExtension();

            //Act
            tasksExtension.Run();
            var result = tasksExtension.ExecutionLog;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<string>));
            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual("+TaskOmega", result[0]);
            Assert.AreEqual("+TaskAlpha", result[1]);
            Assert.IsTrue(result.Contains("+TaskBeta"));
            Assert.IsTrue(result.Contains("+TestStartupTask"));
        }

        [TestMethod]
        public void ShouldResetTasksInReverseSequenceUsingTheTaskAttribute()
        {
            var tasksExtension = new StartupTasksExtension();

            //Act
            tasksExtension.Reset();
            var result = tasksExtension.ExecutionLog;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<string>));
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.Contains("-TestStartupTask"));
            Assert.IsTrue(result.Contains("-TaskBeta"));
            Assert.AreEqual("-TaskAlpha", result[result.Count-2]);
            Assert.AreEqual("-TaskOmega", result[result.Count-1]);
        }

        [TestMethod]
        public void ShouldReturnAStartupTaskOptions()
        {
            //Arrange
            var tasksExtension = new StartupTasksExtension();

            //Act
            var result = tasksExtension.Options;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StartupTasksOptions));
        }

        [TestMethod]
        public void ShouldExecuteTasksInSequenceUsingFluentSequence()
        {
            var tasksExtension = new StartupTasksExtension();
            tasksExtension
                .Options
                    .UsingThisExecutionOrder(s => s
                        .First<TestStartupTask>()
                        .Then<TaskAlpha>()
                        .Then<TaskBeta>()
                        .Then<TaskOmega>()
                        .Then().TheRest());

            //Act
            tasksExtension.Run();
            var result = tasksExtension.ExecutionLog;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<string>));
            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual("+TestStartupTask", result[0]);
            Assert.AreEqual("+TaskAlpha", result[1]);
            Assert.AreEqual("+TaskBeta", result[2]);
            Assert.AreEqual("+TaskOmega", result[3]);
        }

        [TestMethod]
        public void ShouldResetTasksInReverseSequenceUsingFluentSequence()
        {
            var tasksExtension = new StartupTasksExtension();
            tasksExtension
                .Options
                    .UsingThisExecutionOrder(s => s
                        .First<TestStartupTask>()
                        .Then<TaskAlpha>()
                        .Then<TaskBeta>()
                        .Then<TaskOmega>()
                        .Then().TheRest());

            //Act
            tasksExtension.Reset();
            var result = tasksExtension.ExecutionLog;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<string>));
            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual("-TaskOmega", result[0]);
            Assert.AreEqual("-TaskBeta", result[1]);
            Assert.AreEqual("-TaskAlpha", result[2]);
            Assert.AreEqual("-TestStartupTask", result[3]);
        }

        [TestMethod]
        public void ShouldRunUsingFluentPostionOrAttributePositionIfMissingOrDefaultPositionIfBothAreMissing()
        {
            var tasksExtension = new StartupTasksExtension();
            tasksExtension
                .Options
                    .UsingThisExecutionOrder(s => s
                        .First().TheRest()
                        .Then<TaskOmega>());

            //Act
            tasksExtension.Run();
            var result = tasksExtension.ExecutionLog;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<string>));
            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual("+TaskAlpha", result[0]);
            Assert.IsTrue(result.IndexOf("+TaskBeta") < result.IndexOf("+TaskOmega"));
            Assert.IsTrue(result.IndexOf("+TestStartupTask") < result.IndexOf("+TaskOmega"));
            Assert.AreEqual("+TaskOmega", result[result.Count-1]);
        }

        [TestMethod]
        public void ShouldResetUsingFluentPostionOrAttributePositionIfMissingOrDefaultPositionIfBothAreMissing()
        {
            var tasksExtension = new StartupTasksExtension();
            tasksExtension
                .Options
                    .UsingThisExecutionOrder(s => s
                        .First().TheRest()
                        .Then<TaskOmega>());

            //Act
            tasksExtension.Reset();
            var result = tasksExtension.ExecutionLog;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<string>));
            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual("-TaskOmega", result[0]);
            Assert.IsTrue(result.IndexOf("-TaskBeta") > result.IndexOf("-TaskOmega"));
            Assert.IsTrue(result.IndexOf("-TestStartupTask") > result.IndexOf("-TaskOmega"));
            Assert.AreEqual("-TaskAlpha", result[result.Count - 1]);
        }

    }
}
