using System.Collections.Generic;
using System.Threading;
using Bootstrap.Extensions.StartupTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.StartupTasks
{
    [TestClass]
    public class TaskGroupTests
    {
        [TestMethod]
        public void ShouldCreateANewTaskGroup()
        {
            //Arrange
            var tasks = new List<TaskExecutionParameters>();
            var log = new List<ExecutionLogEntry>();
            var thread = new Thread(this.ShouldCreateANewTaskGroup);

            //Act
            var result = new TaskGroup
                             {
                                 Tasks = tasks,
                                 ExecutionLog = log,
                                 Thread = thread
                             };

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TaskGroup));
            Assert.AreSame(tasks, result.Tasks);
            Assert.AreSame(log, result.ExecutionLog);
            Assert.AreSame(thread, result.Thread);
        }

        [TestMethod]
        public void ShouldHaveDefaultValues()
        {
            //Act
            var result = new TaskGroup();

            //Assert
            Assert.IsNotNull(result.Tasks);
            Assert.IsInstanceOfType(result.Tasks, typeof(List<TaskExecutionParameters>));
            Assert.AreEqual(0, result.Tasks.Count);
            Assert.IsNotNull(result.ExecutionLog);
            Assert.IsInstanceOfType(result.ExecutionLog, typeof(List<ExecutionLogEntry>));
            Assert.AreEqual(0, result.ExecutionLog.Count);
            Assert.IsNull(result.Thread);
        }
    }
}
