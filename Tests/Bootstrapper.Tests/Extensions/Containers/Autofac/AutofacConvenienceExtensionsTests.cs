using System.Linq;
using Bootstrap.Autofac;
using Bootstrap.Extensions.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Containers.Autofac
{
    [TestClass]
    public class AutofacConvenienceExtensionsTests
    {
        [TestInitialize]
        [TestCleanup]
        public void Initialize()
        {
            Bootstrapper.ClearExtensions();
        }

        [TestMethod]
        public void ShouldAddTheAutofacExtensionToBootstrapper()
        {
            //Act
            var result = Bootstrapper.With.Autofac();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions().First(), typeof(AutofacExtension));
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(AutofacOptions));
        }

        [TestMethod]
        public void Autofac_WhenInvoked_ShouldPassTheBootstrapperRegistrationHelperToTheConstructorOfTheExtension()
        {
            //Act
            Bootstrapper.With.Autofac();

            //Assert
            var extension = Bootstrapper.GetExtensions().First() as AutofacExtension;
            Assert.IsNotNull(extension);
            Assert.AreSame(Bootstrapper.RegistrationHelper, extension.Registrator);
        }
    }
}
