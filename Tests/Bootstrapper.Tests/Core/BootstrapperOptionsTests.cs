using System.Reflection;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core
{
    [TestClass]
    public class BootstrapperOptionsTests
    {
        private BootstrapperOption opt;

        [TestInitialize]
        [TestCleanup]
        public void InitializeBootstrapper()
        {
            Bootstrapper.ClearExtensions();
        }

        [TestMethod]
        public void Constructor_WhenInvoked_ShouldReturnBootstraperOption()
        {
            //Act
            var result = new BootstrapperOption();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (IBootstrapperOption));
            Assert.IsInstanceOfType(result, typeof (BootstrapperOption));            
        }

        [TestMethod]
        public void With_WhenInvoked_ShouldInvokeWithInBootstrapper()
        {
            //Arrange
            opt = new BootstrapperOption();
            var extension = A.Fake<IBootstrapperExtension>();

            //Act
            opt.With.Extension(extension);

            //Assert
            Assert.IsTrue(Bootstrapper.GetExtensions().Contains(extension));            
        }

        [TestMethod]
        public void Excluding_WhenInvoked_ShouldInvokeExcludingInBootstrapper()
        {
            //Arrange
            opt = new BootstrapperOption();

            //Act
            opt.Excluding.Assembly("test");

            //Assert
            Assert.IsTrue(Bootstrapper.Excluding.Assemblies.Contains("test"));            
        }

        [TestMethod]
        public void Including_WhenInvoked_ShouldInvokeIncludingInBootstrapper()
        {
            //Arrange
            opt = new BootstrapperOption();
            var assembly = Assembly.GetExecutingAssembly();

            //Act
            opt.Including.Assembly(assembly);

            //Assert
            Assert.IsTrue(Bootstrapper.Including.Assemblies.Contains(assembly));            
        }

        [TestMethod]
        public void IncludingOnly_WhenInvoked_ShouldInvokeIncludingOnlyInBootstrapper()
        {
            //Arrange
            opt = new BootstrapperOption();
            var assembly = Assembly.GetExecutingAssembly();

            //Act
            opt.IncludingOnly.Assembly(assembly);

            //Assert
            Assert.IsTrue(Bootstrapper.IncludingOnly.Assemblies.Contains(assembly));            
        }

        [TestMethod]
        public void LookForTypesIn_WhenInvoked_ShouldInvokeLookForTypesInInBootstrapper()
        {
            //Arrange
            opt = new BootstrapperOption();

            //Act
            opt.LookForTypesIn.ReferencedAssemblies();

            //Assert
            var registrationHelper = Bootstrapper.RegistrationHelper as RegistrationHelper;
            Assert.IsNotNull(registrationHelper);            
            Assert.IsTrue(registrationHelper.AssemblyProvider is ReferencedAssemblyProvider);            
        }

        [TestMethod]
        public void Start_WhenInvoked_ShouldInvokeStartInBootstrapper()
        {
            //Arrange
            var extension = A.Fake<IBootstrapperExtension>();
            opt = new BootstrapperOption();

            //Act
            opt.With.Extension(extension).Start();

            //Assert
            A.CallTo(() => extension.Run()).MustHaveHappened();
        }
    }
}
