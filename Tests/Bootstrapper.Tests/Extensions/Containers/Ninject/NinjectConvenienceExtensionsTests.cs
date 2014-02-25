using System.Linq;
using Bootstrap.Extensions.Containers;
using Bootstrap.Ninject;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Containers.Ninject
{
    [TestClass]
    public class NinjectConvenienceExtensionsTests
    {
        [TestInitialize]
        [TestCleanup]
        public void Initialize()
        {
            Bootstrapper.ClearExtensions();
        }

        [TestMethod]
        public void ShouldAddTheNinjectExtensionToBootstrapper()
        {
            //Act
            var result = Bootstrapper.With.Ninject();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[0], typeof(NinjectExtension));
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(NinjectOptions));
        }

        [TestMethod]
        public void Ninject_WhenInvoked_ShouldPassTheBootstrapperRegistrationHelperToTheConstructorOfTheExtension()
        {
            //Act
            Bootstrapper.With.Ninject();

            //Assert
            var extension = Bootstrapper.GetExtensions().First() as NinjectExtension;
            Assert.IsNotNull(extension);
            Assert.AreSame(Bootstrapper.RegistrationHelper, extension.Registrator);
        }
    }
}
