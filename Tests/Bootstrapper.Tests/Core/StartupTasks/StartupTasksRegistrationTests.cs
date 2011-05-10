using Bootstrap.Extensions.Containers;
using Bootstrap.StartupTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bootstrap.Tests.Core.StartupTasks
{
    [TestClass]
    public class StartupTasksRegistrationTests
    {
        [TestMethod]
        public void ShouldCreateANewStartupTasksRegistration()
        {
            //Act
            var result = new StartupTasksRegistration();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(IBootstrapperRegistration));
            Assert.IsInstanceOfType(result, typeof(StartupTasksRegistration));
        }

        [TestMethod]
        public void ShouldInvokeRegisterAllForStartupTasksInContainerExtension()
        {
            //Arrange
            var containerExtension = new Mock<IBootstrapperContainerExtension>();

            //Act
            new StartupTasksRegistration().Register(containerExtension.Object);

            //Assert
            containerExtension.Verify(c => c.RegisterAll<IStartupTask>(), Times.Once());
        }
    }
}
