using Bootstrap.Extensions.StartupTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.StartupTasks
{
    [TestClass]
    public class TaskAttributeTests
    {
        [TestMethod]
        public void ShouldCreateANewTaskAttribute()
        {
            //Act
            var result = new TaskAttribute
                             {
                                 PositionInSequence = 1,
                                 DelayStartBy = 500
                             };

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TaskAttribute));
            Assert.AreEqual(1, result.PositionInSequence);
            Assert.AreEqual(500, result.DelayStartBy);
        }

        [TestMethod]
        public void ShouldReturnDefaultValues()
        {
            //Act
            var result = new TaskAttribute();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TaskAttribute));
            Assert.AreEqual(int.MaxValue, result.PositionInSequence);
            Assert.AreEqual(0, result.DelayStartBy);
        }

    }
}
