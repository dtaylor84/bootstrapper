using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Bootstrap.Extensions.StartupTasks;
using Bootstrap.Tests.Extensions.TestImplementations;
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

        [TestMethod]
        public void ShouldReturnNonDynamicAssemblies()
        {
            //Act
            var result = RegistrationHelper.GetAssemblies().ToList();

            //Assert
            Assert.IsFalse(result.Any(a => a.IsDynamic));
        }

        [TestMethod]
        public void ShouldExcludeExcludedAssemblies()
        {
            //Arrange
            Bootstrapper.Excluding.Assembly("StructureMap").AndAssembly("Castle").AndAssembly("Windsor");
            //Act
            var result = RegistrationHelper.GetAssemblies().ToList();

            //Assert
            Bootstrapper.Excluding.Assemblies.ForEach(e => Assert.IsFalse(result.Any(a => a.FullName.StartsWith(e))));
        }

        [TestMethod]
        public void ShouldReturnInstancesOfAType()
        {
            //Act
            var result = RegistrationHelper.GetInstancesOfTypesImplementing<IBootstrapperContainerExtension>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<IBootstrapperContainerExtension>));
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.Any(c => c is TestContainerExtension));
        }
    }
}
