using System.Linq;
using Bootstrap.Extensions.Containers;
using Bootstrap.Windsor;
using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Containers.Windsor
{
    [TestClass]
    public class BootstrapperWindsorHelperTests
    {
        [TestMethod]
        public void ShouldAddTheWindsorExtensionToBootstrapper()
        {
            //Arrange
            Bootstrapper.ClearExtensions();

            //Act
            var result = Bootstrapper.With.Windsor();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[0], typeof (WindsorExtension));
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof (BootstrapperContainerExtensionOptions));
        }

        [TestMethod]
        public void ShouldRegisterRequestedFacilities()
        {
            //Arrange
            Bootstrapper.ClearExtensions();

            //Act
            Bootstrapper.With.Windsor(
                Facilities
                    .Include<TypedFactoryFacility>()
                    .Select())
                .Start();

            //Assert
            var container = ((IWindsorContainer)Bootstrapper.Container);
            Assert.IsTrue(container.Kernel.GetFacilities().Any(f => f is TypedFactoryFacility));
        }
    }
}
