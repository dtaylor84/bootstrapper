using Bootstrap.Extensions.Containers;
using Bootstrap.Extensions.StartupTasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.StartupTasks
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
            var containerExtension = A.Fake<IBootstrapperContainerExtension>();

            //Act
            new StartupTasksRegistration().Register(containerExtension);

            //Assert
            A.CallTo(() => containerExtension.RegisterAll<IStartupTask>()).MustHaveHappened();
        }
    }
}
