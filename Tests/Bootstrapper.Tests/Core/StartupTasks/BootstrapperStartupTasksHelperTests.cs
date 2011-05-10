using System.Collections.Generic;
using Bootstrap.Extensions.Containers;
using Bootstrap.StartupTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
            var containerExtension = new Mock<IBootstrapperContainerExtension>();
            containerExtension.Setup(c => c.ResolveAll<IStartupTask>()).Returns(new List<IStartupTask>());

            //Act
            Bootstrapper.With.Extension(containerExtension.Object).And.StartupTasks();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[1], typeof(StartupTasksExtension));
        }
    }
}
