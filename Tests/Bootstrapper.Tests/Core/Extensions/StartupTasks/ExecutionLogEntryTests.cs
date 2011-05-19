using System;
using Bootstrap.Extensions.StartupTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.StartupTasks
{
    [TestClass]
    public class ExecutionLogEntryTests
    {
        [TestMethod]
        public void ShouldCreateANewExecutionLogEntry()
        {
            //Arrange
            var now = DateTime.Now;

            //Act
            var result = new ExecutionLogEntry
                             {
                                 Timestamp = now,
                                 TaskName = "Test",
                                 SequencePosition = 1,
                                 DelayInMilliseconds = 200,
                                 StartedAt = now.AddMilliseconds(200),
                                 EndedAt = now.AddMilliseconds(300)
                             };

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ExecutionLogEntry));
            Assert.AreEqual(now, result.Timestamp);
            Assert.AreEqual("Test", result.TaskName);
            Assert.AreEqual(1, result.SequencePosition);
            Assert.AreEqual(200, result.DelayInMilliseconds);
            Assert.AreEqual(now.AddMilliseconds(200), result.StartedAt);
            Assert.AreEqual(now.AddMilliseconds(300), result.EndedAt);
        }
    }
}
