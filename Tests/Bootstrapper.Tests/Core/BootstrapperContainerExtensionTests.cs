using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Bootstrap.Tests.ExtensionForTest;
using Bootstrap.Tests.Extensions.Containers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bootstrap.Tests.Core
{
    [TestClass]
    public class BootstrapperContainerExtensionTests
    {

        [TestMethod]
        public void ShouldCreateATestContainerExtension()
        {
            //Act
            var result = new TestContainerExtension(null);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TestContainerExtension));
        }

        [TestMethod]
        public void ShouldLookForRegistrationsInBootstrapperAssembly()
        {
            //Arrange
            var containerExtension = new TestContainerExtension(null);
            var locator = new Mock<IServiceLocator>();
            containerExtension.SetTestServiceLocator(locator.Object);
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act

            containerExtension.Run();
            var assemblies = containerExtension.LookForRegistrations.Assemblies;
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsTrue(assemblies.Any(a =>
                a == Assembly.GetAssembly(typeof(Bootstrap.Bootstrapper))));
        }

        [TestMethod]
        public void ShouldLookForMapsInBootstrapperAssembly()
        {
            //Arrange
            var containerExtension = new TestContainerExtension(null);
            var locator = new Mock<IServiceLocator>();
            containerExtension.SetTestServiceLocator(locator.Object);
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var assemblies = containerExtension.LookForMaps.Assemblies;
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsTrue(assemblies.Any(a =>
                a == Assembly.GetAssembly(typeof(Bootstrap.Bootstrapper))));
        }

        [TestMethod]
        public void ShouldLookForStartupTasksInBootstrapperAssembly()
        {
            //Arrange
            var containerExtension = new TestContainerExtension(null);
            var locator = new Mock<IServiceLocator>();
            containerExtension.SetTestServiceLocator(locator.Object);
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var assemblies = containerExtension.LookForStartupTasks.Assemblies;
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsTrue(assemblies.Any(a =>
                a == Assembly.GetAssembly(typeof(Bootstrap.Bootstrapper))));
        }

        [TestMethod]
        public void ShouldLookForRegistrationsInContainerExtensionAssembly()
        {
            //Arrange
            var containerExtension = new TestContainerExtension(null);
            var locator = new Mock<IServiceLocator>();
            containerExtension.SetTestServiceLocator(locator.Object);
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var assemblies = containerExtension.LookForRegistrations.Assemblies;
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsTrue(assemblies.Any(a =>
                a == Assembly.GetAssembly(typeof(TestContainerExtension))));
        }

        [TestMethod]
        public void ShouldLookForMapsInContainerExtensionAssembly()
        {
            //Arrange
            var containerExtension = new TestContainerExtension(null);
            var locator = new Mock<IServiceLocator>();
            containerExtension.SetTestServiceLocator(locator.Object);
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var assemblies = containerExtension.LookForMaps.Assemblies;
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsTrue(assemblies.Any(a =>
                a == Assembly.GetAssembly(typeof(TestContainerExtension))));
        }

        [TestMethod]
        public void ShouldLookForStartupTasksInContainerExtensionAssembly()
        {
            //Arrange
            var containerExtension = new TestContainerExtension(null);
            var locator = new Mock<IServiceLocator>();
            containerExtension.SetTestServiceLocator(locator.Object);
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var assemblies = containerExtension.LookForStartupTasks.Assemblies;
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsTrue(assemblies.Any(a =>
                a == Assembly.GetAssembly(typeof(TestContainerExtension))));
        }

        [TestMethod]
        public void ShouldLookForRegistrationsInBootstrapperStartCallingAssembly()
        {
            //Arrange
            Bootstrap.Bootstrapper.ClearExtensions();
            var containerExtension = new TestContainerExtension(null);
            var locator = new Mock<IServiceLocator>();
            containerExtension.SetTestServiceLocator(locator.Object);
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            Bootstrap.Bootstrapper.Start();
            var assemblies = containerExtension.LookForRegistrations.Assemblies;
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert            
            Assert.IsTrue(assemblies.Any(a =>
                a == Assembly.GetExecutingAssembly()));
        }

        [TestMethod]
        public void ShouldLookForMapsInBootstrapperStartCallingAssembly()
        {
            //Arrange
            Bootstrap.Bootstrapper.ClearExtensions();
            var containerExtension = new TestContainerExtension(null);
            var locator = new Mock<IServiceLocator>();
            containerExtension.SetTestServiceLocator(locator.Object);
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            Bootstrap.Bootstrapper.Start();
            var assemblies = containerExtension.LookForMaps.Assemblies;
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsTrue(assemblies.Any(a =>
                a == Assembly.GetExecutingAssembly()));
        }

        [TestMethod]
        public void ShouldLookForStartupTasksInBootstrapperStartCallingAssembly()
        {
            //Arrange
            Bootstrap.Bootstrapper.ClearExtensions();
            var containerExtension = new TestContainerExtension(null);
            var locator = new Mock<IServiceLocator>();
            containerExtension.SetTestServiceLocator(locator.Object);
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            Bootstrap.Bootstrapper.Start();
            var assemblies = containerExtension.LookForStartupTasks.Assemblies;
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsTrue(assemblies.Any(a =>
                a == Assembly.GetExecutingAssembly()));
        }

        //NOTE: Couldn't add tests for Entry Assembly since GetEntryAssembly returns null in a test assembly

        [TestMethod]
        public void ShouldLookForRegistrationsInExtensionsAssembly()
        {
            //Arrange
            Bootstrap.Bootstrapper.ClearExtensions();
            var containerExtension = new TestContainerExtension(null);
            var locator = new Mock<IServiceLocator>();
            containerExtension.SetTestServiceLocator(locator.Object);
            Bootstrap.Bootstrapper.With.Container(containerExtension);
            Bootstrap.Bootstrapper.With.Extension(new TestBootstrapperExtension());

            //Act
            Bootstrap.Bootstrapper.Start();
            var assemblies = containerExtension.LookForRegistrations.Assemblies;
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert            
            Assert.IsTrue(assemblies.Any(a =>
                a == Assembly.GetAssembly(typeof(TestBootstrapperExtension))));
        }

        [TestMethod]
        public void ShouldLookForMapsInExtensionsAssembly()
        {
            //Arrange
            Bootstrap.Bootstrapper.ClearExtensions();
            var containerExtension = new TestContainerExtension(null);
            var locator = new Mock<IServiceLocator>();
            containerExtension.SetTestServiceLocator(locator.Object);
            Bootstrap.Bootstrapper.With.Container(containerExtension);
            Bootstrap.Bootstrapper.With.Extension(new TestBootstrapperExtension());

            //Act
            Bootstrap.Bootstrapper.Start();
            var assemblies = containerExtension.LookForMaps.Assemblies;
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert            
            Assert.IsTrue(assemblies.Any(a =>
                a == Assembly.GetAssembly(typeof(TestBootstrapperExtension))));
        }

        [TestMethod]
        public void ShouldLookForStartupTasksInExtensionsAssembly()
        {
            //Arrange
            Bootstrap.Bootstrapper.ClearExtensions();
            var containerExtension = new TestContainerExtension(null);
            var locator = new Mock<IServiceLocator>();
            containerExtension.SetTestServiceLocator(locator.Object);
            Bootstrap.Bootstrapper.With.Container(containerExtension);
            Bootstrap.Bootstrapper.With.Extension(new TestBootstrapperExtension());

            //Act
            Bootstrap.Bootstrapper.Start();
            var assemblies = containerExtension.LookForStartupTasks.Assemblies;
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert            
            Assert.IsTrue(assemblies.Any(a =>
                a == Assembly.GetAssembly(typeof(TestBootstrapperExtension))));
        }

        [TestMethod]
        public void ShouldSetTheBootstrapperContainer()
        {
            //Arrange
            Bootstrap.Bootstrapper.ClearExtensions();
            var container = new object();
            var containerExtension = new TestContainerExtension(container);
            var locator = new Mock<IServiceLocator>();
            containerExtension.SetTestServiceLocator(locator.Object);
            Bootstrap.Bootstrapper.With.Container(containerExtension);

            //Act
            containerExtension.Run();
            var result = Bootstrap.Bootstrapper.GetContainer();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(object));
            Assert.AreSame(container, result);
        }

        [TestMethod]
        public void ShouldInvokeTheRunMethodOfAllIStartupTaskTypes()
        {
            //Arrange
            TestStartupTask.Invoked = false;
            var container = new object();
            var locator = new Mock<IServiceLocator>();
            locator.Setup(l => l.GetAllInstances<IStartupTask>()).Returns(new List<IStartupTask> {new TestStartupTask()});
            var containerExtension = new TestContainerExtension(container);
            containerExtension.SetTestServiceLocator(locator.Object);
            Bootstrap.Bootstrapper.With.Container(containerExtension).Start();

            //Act
            var result = TestStartupTask.Invoked;
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ShouldInvokeTheResetMethodOfAllIStartupTaskTypes()
        {
            //Arrange
            var container = new object();
            var locator = new Mock<IServiceLocator>();
            locator.Setup(l => l.GetAllInstances<IStartupTask>()).Returns(new List<IStartupTask> { new TestStartupTask() });
            var containerExtension = new TestContainerExtension(container);
            containerExtension.SetTestServiceLocator(locator.Object);

            //Act
            containerExtension.Reset();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsFalse(TestStartupTask.Invoked);
        }        

        [TestMethod]
        public void ShouldResetTheBootstrapperContainer()
        {
            //Arrange
            var containerExtension = new TestContainerExtension(new object());
            var locator = new Mock<IServiceLocator>();
            containerExtension.SetTestServiceLocator(locator.Object);
            Bootstrap.Bootstrapper.With.Container(containerExtension).Start();

            //Act
            containerExtension.Reset();
            var result = Bootstrap.Bootstrapper.GetContainer();
            Bootstrap.Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNull(result);
        }


    }
}
