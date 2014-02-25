using System.Linq;
using Bootstrap.Extensions.Containers;
using Bootstrap.SimpleInjector;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Containers.SimpleInjector
{
    [TestClass]
    public class SimpleInjectorConvenienceExtensionsTests
    {
        [TestInitialize]
        [TestCleanup]
        public void InitializeBootstrapper()
        {
            Bootstrapper.ClearExtensions();
        }

        [TestMethod]
        public void SimpleInjector_WhenInvoked_ShouldAddTheSimpleInjectorExtensionToBootstrapper()
        {
            //Act
            var result = Bootstrapper.With.SimpleInjector();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[0], typeof(SimpleInjectorExtension));
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(SimpleInjectorOptions));
        }

        [TestMethod]
        public void SimpleInjector_WhenInvoked_ShouldPassTheBootstrapperRegistrationHelperToTheConstructorOfTheExtension()
        {
            //Act
            Bootstrapper.With.SimpleInjector();

            //Assert
            var extension = Bootstrapper.GetExtensions().First() as SimpleInjectorExtension;
            Assert.IsNotNull(extension);
            Assert.AreSame(Bootstrapper.RegistrationHelper, extension.Registrator);
        }
    }
}
