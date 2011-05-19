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
            var result = Bootstrapper.With.Extension(containerExtension).And.StartupTasks();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StartupTasksOptions));
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[1], typeof(StartupTasksExtension));
            var extension = Bootstrapper.GetExtensions()[1] as StartupTasksExtension;
            Assert.IsNotNull(extension);
            Assert.AreSame(extension.Options, result);
        }
    }
}
