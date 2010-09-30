using System.Reflection;
using System.Linq;
using Bootstrap.Tests.Extensions.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core
{
    [TestClass]
    public class RegistrationHelperTests
    {
        [TestMethod]
        public void ShouldReturnTypesFromAGivenAssembly()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof (AssemblyCollector));

            //Act
            var result = RegistrationHelper.GetTypesImplementing<IAssemblyCollector>(assembly);

            //Assert
            Assert.IsTrue(result.Any(t => t == typeof(AssemblyCollector)));
        }

        [TestMethod]
        public void ShouldReturnTypesFromAGiveAssemblyName()
        {
            //Arrange
            var assemblyName = Assembly.GetAssembly(typeof(AssemblyCollector)).GetName().FullName;

            //Act
            var result = RegistrationHelper.GetTypesImplementing<IAssemblyCollector>(assemblyName);

            //Assert
            Assert.IsTrue(result.Any(t => t == typeof(AssemblyCollector)));            
        }

        [TestMethod]
        public void ShouldReturnTypesFromCallingAssembly()
        {
            //Act
            var result = RegistrationHelper.GetTypesImplementing<IStartupTask>();

            //Assert
            Assert.IsTrue(result.Any(t => t == typeof(TestStartupTask)));
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
