using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Bootstrap.Extensions.StartupTasks;
using Bootstrap.Tests.Extensions.TestImplementations;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.StartupTasks
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
            Assert.IsInstanceOfType(result, typeof(List<ExecutionLogEntry>));
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
            Assert.IsInstanceOfType(result, typeof(List<ExecutionLogEntry>));
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.Any(e => e.TaskName == "+TaskOmega"));
            Assert.IsTrue(result.Any(e => e.TaskName == "+TaskAlpha"));
            Assert.IsTrue(result.Any(e => e.TaskName == "+TaskBeta"));
            Assert.IsTrue(result.Any(e => e.TaskName == "+TestStartupTask"));
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
            Assert.IsInstanceOfType(result, typeof(List<ExecutionLogEntry>));
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.Any(e => e.TaskName == "-TaskOmega"));
            Assert.IsTrue(result.Any(e => e.TaskName == "-TaskAlpha"));
            Assert.IsTrue(result.Any(e => e.TaskName == "-TaskBeta"));
            Assert.IsTrue(result.Any(e => e.TaskName == "-TestStartupTask"));
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
            Assert.IsInstanceOfType(result, typeof(List<ExecutionLogEntry>));
            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual("+TaskOmega", result[0].TaskName);
            Assert.AreEqual("+TaskAlpha", result[1].TaskName);
            Assert.IsTrue(result.Any(e => e.TaskName == "+TaskBeta"));
            Assert.IsTrue(result.Any(e => e.TaskName == "+TestStartupTask"));
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
            Assert.IsInstanceOfType(result, typeof(List<ExecutionLogEntry>));
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.Any(e => e.TaskName == "-TestStartupTask"));
            Assert.IsTrue(result.Any(e => e.TaskName == "-TaskBeta"));
            Assert.AreEqual("-TaskAlpha", result[result.Count-2].TaskName);
            Assert.AreEqual("-TaskOmega", result[result.Count-1].TaskName);
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
            Assert.IsInstanceOfType(result, typeof(List<ExecutionLogEntry>));
            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual("+TestStartupTask", result[0].TaskName);
            Assert.AreEqual("+TaskAlpha"      , result[1].TaskName);
            Assert.AreEqual("+TaskBeta"       , result[2].TaskName);
            Assert.AreEqual("+TaskOmega"      , result[3].TaskName);
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
            Assert.IsInstanceOfType(result, typeof(List<ExecutionLogEntry>));
            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual("-TaskOmega"      , result[0].TaskName);
            Assert.AreEqual("-TaskBeta"       , result[1].TaskName);
            Assert.AreEqual("-TaskAlpha"      , result[2].TaskName);
            Assert.AreEqual("-TestStartupTask", result[3].TaskName);
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
            Assert.IsInstanceOfType(result, typeof(List<ExecutionLogEntry>));
            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual("+TaskAlpha", result[0].TaskName);
            Assert.IsTrue(result.FindIndex(e => e.TaskName == "+TaskBeta") < result.FindIndex(e => e.TaskName == "+TaskOmega"));
            Assert.IsTrue(result.FindIndex(e => e.TaskName == "+TestStartupTask") < result.FindIndex(e => e.TaskName == "+TaskOmega"));
            Assert.AreEqual("+TaskOmega", result[result.Count-1].TaskName);
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
            Assert.IsInstanceOfType(result, typeof(List<ExecutionLogEntry>));
            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual("-TaskOmega", result[0].TaskName);
            Assert.IsTrue(result.FindIndex(e => e.TaskName == "-TaskBeta") > result.FindIndex(e => e.TaskName == "-TaskOmega"));
            Assert.IsTrue(result.FindIndex(e => e.TaskName == "-TestStartupTask") > result.FindIndex(e => e.TaskName == "-TaskOmega"));
            Assert.AreEqual("-TaskAlpha", result[result.Count - 1].TaskName);
        }

        [TestMethod]
        public void ShouldDelayStartOfTaskWhenDeclaredInAttribute()
        {
            var taskExtension = new StartupTasksExtension();

            //Act
            taskExtension.Run();
            var result = taskExtension.ExecutionLog;

            //Assert
            var beta = result.First(e => e.TaskName == "+TaskBeta");
            Assert.IsTrue(beta.StartedAt >= beta.Timestamp.AddMilliseconds(beta.DelayInMilliseconds));
        }

        [TestMethod]
        public void ShouldNotDelayResetOfTaskWhenDeclaredInAttribute()
        {
            var taskExtension = new StartupTasksExtension();

            //Act
            taskExtension.Reset();
            var result = taskExtension.ExecutionLog;

            //Assert
            var beta = result.First(e => e.TaskName == "-TaskBeta");
            Assert.AreEqual(0, beta.DelayInMilliseconds);
            Assert.IsTrue(beta.StartedAt >= beta.Timestamp.AddMilliseconds(beta.DelayInMilliseconds));
        }

    }
}
