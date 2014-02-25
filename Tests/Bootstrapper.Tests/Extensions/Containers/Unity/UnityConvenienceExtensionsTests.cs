using System.Linq;
using Bootstrap.Extensions.Containers;
using Bootstrap.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Containers.Unity
{
    [TestClass]
    public class UnityConvenienceExtensionsTests
    {
        [TestInitialize]
        [TestCleanup]
        public void InitializeBootstrapper()
        {
            Bootstrapper.ClearExtensions();
        }
        
        [TestMethod]
        public void ShouldAddTheUnityExtensionToBootstrapper()
        {
            //Act
            var result = Bootstrapper.With.Unity();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[0], typeof(UnityExtension));
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(UnityOptions));
        }

        [TestMethod]
        public void Unity_WhenInvoked_ShouldPassTheBootstrapperRegistrationHelperToTheConstructorOfTheExtension()
        {
            //Act
            Bootstrapper.With.Unity();

            //Assert
            var extension = Bootstrapper.GetExtensions().First() as UnityExtension;
            Assert.IsNotNull(extension);
            Assert.AreSame(Bootstrapper.RegistrationHelper, extension.Registrator);
        }
    }
}
