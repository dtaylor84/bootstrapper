using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions.Containers;
using Bootstrap.Extensions.StartupTasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.StartupTasks
{
    [TestClass]
    public class StartupTasksConvenienceExtensionsTests
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

        [TestMethod]
        public void StartupTasks_WhenInvoked_ShouldPassTheBootstrapperRegistrationHelperToTheConstructorOfTheExtension()
        {
            //Act
            Bootstrapper.With.StartupTasks();

            //Assert
            var extension = Bootstrapper.GetExtensions().First() as StartupTasksExtension;
            Assert.IsNotNull(extension);            
            Assert.AreSame(Bootstrapper.RegistrationHelper, extension.Registrator);            
        }
    }
}
