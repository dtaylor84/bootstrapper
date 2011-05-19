using Bootstrap.Extensions.StartupTasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.StartupTasks
{
    [TestClass]
    public class SequenceSpecialTests
    {
        [TestMethod]
        public void ShouldCreateASequenceSpecial()
        {
            //Arrange
            var spec = A.Fake<ISequenceSpecification>();

            //Act
            var result = new SequenceSpecial(spec, true);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ISequenceSpecial));
            Assert.IsInstanceOfType(result, typeof(SequenceSpecial));
        }

        [TestMethod]
        public void ShouldInvokeFirstWithIStartupTask()
        {
            //Arrange
            var spec = A.Fake<ISequenceSpecification>();
            var special = new SequenceSpecial(spec, true);

            //Act
            var result = special.TheRest();

            //Assert
            Assert.AreSame(spec, special.SequenceSpecification);
            Assert.AreEqual(true, special.FirstInSequence);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ISequenceSpecification));
            Assert.AreSame(spec, result);
            A.CallTo(() => spec.First<IStartupTask>()).MustHaveHappened();
        }

        [TestMethod]
        public void ShouldInvokeThenWithIStartupTask()
        {
            //Arrange
            var spec = A.Fake<ISequenceSpecification>();
            var special = new SequenceSpecial(spec, false);

            //Act
            var result = special.TheRest();

            //Assert
            Assert.AreSame(spec, special.SequenceSpecification);
            Assert.AreEqual(false, special.FirstInSequence);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ISequenceSpecification));
            Assert.AreSame(spec, result);
            A.CallTo(() => spec.Then<IStartupTask>()).MustHaveHappened();
        }

    }
}
