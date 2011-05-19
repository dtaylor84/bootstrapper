using System.Collections.Generic;
using Bootstrap.Extensions.StartupTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.StartupTasks
{
    [TestClass]
    public class StartupTasksOptionsTests
    {
        [TestMethod]
        public void ShouldCreateANewStartupTasksOptions()
        {
            //Act
            var result = new StartupTasksOptions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StartupTasksOptions));
        }

        [TestMethod]
        public void ShouldReturnAnEmptySequence()
        {
            //Act
            var options = new StartupTasksOptions();
            var result = options.Sequence;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<TaskExecutionParameters>));
        }

        [TestMethod]
        public void ShouldReturnASequenceSpecification()
        {
            //Act
            var options = new StartupTasksOptions();
            options.UsingThisExecutionOrder(s => s.First<TaskAlpha>().Then<TaskBeta>());
            var result = options.Sequence;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<TaskExecutionParameters>));
            Assert.AreEqual(typeof(TaskAlpha), result[0].TaskType);
            Assert.AreEqual(typeof(TaskBeta), result[1].TaskType);
        }
    }
}
