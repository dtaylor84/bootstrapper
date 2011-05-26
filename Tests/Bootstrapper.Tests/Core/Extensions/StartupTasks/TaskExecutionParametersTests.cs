using Bootstrap.Extensions.StartupTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.StartupTasks
{
    [TestClass]
    public class TaskExecutionParametersTests
    {
        [TestMethod]
        public void ShouldCreateANewStartupTaskExecutionParamter()
        {
            //Arrange
            var task = new TaskAlpha();

            //Act
            var result = new TaskExecutionParameters
                             {
                                 Task = task,
                                 TaskType = task.GetType(),
                                 Position = 1,
                                 Delay = 100,
                                 Group = 2
                             };

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TaskExecutionParameters));
            Assert.AreSame(task, result.Task);
            Assert.AreSame(typeof(TaskAlpha), result.TaskType);
            Assert.AreEqual(1, result.Position);
            Assert.AreEqual(100, result.Delay);
            Assert.AreEqual(2, result.Group);
        }
    }
}
