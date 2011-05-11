using System.Collections.Generic;
using Bootstrap.Extensions.Containers;
using Bootstrap.StartupTasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.StartupTasks
{
    [TestClass]
    public class BootstrapperStartupTasksHelperTests
    {
        [TestInitialize]
        [TestCleanup]
        public void Initialize()
        {
            Bootstrapper.ClearExtensions();
        }

        [TestMethod]
        public void ShouldAddTheStartupTaskExtensionToBootstrapper()
        {
            //Arrange
            Bootstrapper.ClearExtensions();
            var containerExtension = A.Fake<IBootstrapperContainerExtension>();
            A.CallTo(() => containerExtension.ResolveAll<IStartupTask>()).Returns(new List<IStartupTask>());

            //Act
            Bootstrapper.With.Extension(containerExtension).And.StartupTasks();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[1], typeof(StartupTasksExtension));
        }
    }
}
