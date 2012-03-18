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
        private IRegistrationHelper registrationHelper;

        [TestInitialize]
        public void Initialize()
        {
            registrationHelper = new RegistrationHelper();
        }

        [TestMethod]
        public void ShouldCreateANewRegistrationHelper()
        {
            //Assert
            Assert.IsNotNull(registrationHelper);
            Assert.IsInstanceOfType(registrationHelper, typeof(IRegistrationHelper));
            Assert.IsInstanceOfType(registrationHelper, typeof(RegistrationHelper));
        }

        [TestMethod]
        public void ShouldReturnTypesFromAGivenAssembly()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof (IBootstrapperExtension));

            //Act
            var result = registrationHelper.GetTypesImplementing<IBootstrapperExtension>(assembly);

            //Assert
            Assert.IsTrue(result.Any(t => typeof(IBootstrapperExtension).IsAssignableFrom(t)));
        }

        [TestMethod]
        public void ShouldReturnTypesFromAGiveAssemblyName()
        {
            //Arrange
            var assemblyName = Assembly.GetAssembly(typeof(IBootstrapperExtension)).GetName().FullName;

            //Act
            var result = registrationHelper.GetTypesImplementing<IBootstrapperExtension>(assemblyName);

            //Assert
            Assert.IsTrue(result.Any(t => typeof(IBootstrapperExtension).IsAssignableFrom(t)));            
        }

        [TestMethod]
        public void ShouldReturnTypesFromCallingAssembly()
        {
            //Act
            var result = registrationHelper.GetTypesImplementing<IStartupTask>();

            //Assert
            Assert.IsTrue(result.Any(t => typeof(TestStartupTask).IsAssignableFrom(t)));
        }

        [TestMethod]
        public void ShouldExcludeNonPublicClasses()
        {
            //Act
            var result = registrationHelper.GetTypesImplementing<IStartupTask>();

            //Assert
            Assert.IsFalse(result.Any(t => t == typeof(InternalTestStartupTask)));            
        }
        
        [TestMethod]
        public void ShouldExcludeAbstractClasses()
        {
            //Act
            var result = registrationHelper.GetTypesImplementing<IStartupTask>();

            //Assert
            Assert.IsFalse(result.Any(t => t == typeof(AbstractTestStartupTask)));
        }

        [TestMethod]
        public void ShouldReturnNonDynamicAssemblies()
        {
            //Act
            var result = registrationHelper.GetAssemblies().ToList();

            //Assert
            Assert.IsFalse(result.Any(a => a.IsDynamic));
        }

        [TestMethod]
        public void ShouldExcludeExcludedAssemblies()
        {
            //Arrange
            Bootstrapper.Excluding.Assembly("StructureMap").AndAssembly("Castle").AndAssembly("Windsor");

            //Act
            var result = registrationHelper.GetAssemblies().ToList();

            //Assert
            Bootstrapper.Excluding.Assemblies.ForEach(e => Assert.IsFalse(result.Any(a => a.FullName.StartsWith(e))));
        }

        [TestMethod]
        public void ShouldIncludeOnlyIncludedOnlyAssemblies()
        {
            //Arrange
            var systemAssembly = Assembly.GetAssembly(typeof (System.DateTime));
            Bootstrapper.IncludingOnly.Assembly(systemAssembly);

            //Act
            var result = registrationHelper.GetAssemblies().ToList();

            //Assert
            Assert.IsTrue(result.SequenceEqual(Bootstrapper.IncludingOnly.Assemblies));
        }

        [TestMethod]
        public void ShouldExcluedExcludedAssembliesExceptoForTheExplicitelyIncludedOnes()
        {
            //Arrange
            Bootstrapper.Excluding.Assembly("Bootstrapper").Including.Assembly(Assembly.GetExecutingAssembly());

            //Act
            var result = registrationHelper.GetAssemblies().ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count(a => a.FullName.StartsWith("Bootstrapper")));
            Assert.AreEqual(Assembly.GetExecutingAssembly(), result.First(a => a.FullName.StartsWith("Bootstrapper")));
        }

        [TestMethod]
        public void ShouldReturnInstancesOfAType()
        {
            //Act
            var result = registrationHelper.GetInstancesOfTypesImplementing<IRegistrationHelper>();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<IRegistrationHelper>));
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.Any(c => c is RegistrationHelper));
        }
    }
}
