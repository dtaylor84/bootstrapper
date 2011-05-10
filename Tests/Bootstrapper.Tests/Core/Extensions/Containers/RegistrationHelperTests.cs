using System.Reflection;
using System.Linq;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Bootstrap.Tests.Extensions.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.Containers
{
    [TestClass]
    public class RegistrationHelperTests
    {
        [TestMethod]
        public void ShouldReturnTypesFromAGivenAssembly()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof (IBootstrapperExtension));

            //Act
            var result = RegistrationHelper.GetTypesImplementing<IBootstrapperExtension>(assembly);

            //Assert
            Assert.IsTrue(result.Any(t => typeof(IBootstrapperExtension).IsAssignableFrom(t)));
        }

        [TestMethod]
        public void ShouldReturnTypesFromAGiveAssemblyName()
        {
            //Arrange
            var assemblyName = Assembly.GetAssembly(typeof(IBootstrapperExtension)).GetName().FullName;

            //Act
            var result = RegistrationHelper.GetTypesImplementing<IBootstrapperExtension>(assemblyName);

            //Assert
            Assert.IsTrue(result.Any(t => typeof(IBootstrapperExtension).IsAssignableFrom(t)));            
        }

        [TestMethod]
        public void ShouldReturnTypesFromCallingAssembly()
        {
            //Act
            var result = RegistrationHelper.GetTypesImplementing<IStartupTask>();

            //Assert
            Assert.IsTrue(result.Any(t => typeof(TestStartupTask).IsAssignableFrom(t)));
        }

        [TestMethod]
        public void ShouldExcludeNonPublicClasses()
        {
            //Act
            var result = RegistrationHelper.GetTypesImplementing<IStartupTask>();

            //Assert
            Assert.IsFalse(result.Any(t => t == typeof(InternalTestStartupTask)));            
        }

        [TestMethod]
        public void ShouldExcludeAbstractClasses()
        {
            //Act
            var result = RegistrationHelper.GetTypesImplementing<IStartupTask>();

            //Assert
            Assert.IsFalse(result.Any(t => t == typeof(AbstractTestStartupTask)));
        }

    }
}
