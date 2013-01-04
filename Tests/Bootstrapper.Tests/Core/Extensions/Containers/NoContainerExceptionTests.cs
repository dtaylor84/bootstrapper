using Bootstrap.Extensions.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.Containers
{
    [TestClass]
    public class NoContainerExceptionTests
    {
        [TestMethod]
        public void ShouldCreateANewNoContainerException()
        {
            //Act
            var result = new NoContainerException();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContainerException));
            Assert.AreEqual(NoContainerException.DefaultMessage, result.Message);
        }

        [TestMethod]
        public void ShouldCreateANewNoContainerExceptionWithCustomMessage()
        {
            //Act
            var result = new NoContainerException("Test");

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContainerException));
            Assert.AreEqual("Test", result.Message);
        }

    }
}
