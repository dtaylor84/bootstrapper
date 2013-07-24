using System.Linq;
using Bootstrap.Extensions.Containers;
using Bootstrap.Windsor;
using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Containers.Windsor
{
    [TestClass]
    public class WindsorConvenienceExtensionsTests
    {
        [TestInitialize]
        [TestCleanup]
        public void InitializeBootstrapper()
        {
            Bootstrapper.ClearExtensions();
        }

        [TestMethod]
        public void ShouldAddTheWindsorExtensionToBootstrapper()
        {
            //Act
            var result = Bootstrapper.With.Windsor();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[0], typeof (WindsorExtension));
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof (WindsorOptions));
        }

        [TestMethod]
        public void ShouldRegisterRequestedFacilities()
        {
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

        [TestMethod]
        public void Windsor_WhenInvoked_ShouldPassTheBootstrapperRegistrationHelperToTheConstructorOfTheExtension()
        {
            //Act
            Bootstrapper.With.Windsor();

            //Assert
            var extension = Bootstrapper.GetExtensions().First() as WindsorExtension;
            Assert.IsNotNull(extension);
            Assert.AreSame(Bootstrapper.RegistrationHelper, extension.Registrator);
        }
    }
}
