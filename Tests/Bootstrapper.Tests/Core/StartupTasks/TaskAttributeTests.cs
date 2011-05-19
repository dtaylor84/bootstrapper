using Bootstrap.StartupTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.StartupTasks
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
                                 PositionInSequence = 1
                             };

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TaskAttribute));
            Assert.AreEqual(1, result.PositionInSequence);
        }
    }
}
