using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core
{
    [TestClass]
    public class IncludedOnlyAssembliesTests
    {
        [TestMethod]
        public void ShouldCreateANewIncludedOnlyAssemblies()
        {
            //Act
            var result = new IncludedOnlyAssemblies();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IIncludedOnlyAssemblies));
            Assert.IsInstanceOfType(result, typeof(IncludedOnlyAssemblies));
        }

        [TestMethod]
        public void ShouldReturnEmpyListOfAssemblies()
        {
            //Act
            var result = new IncludedOnlyAssemblies().Assemblies;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Assembly>));
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void ShouldAddAssemblyToIncludedOnlyAssemblies()
        {
            //Act
            var includedOnly = new IncludedOnlyAssemblies();
            var result = includedOnly.Assembly(Assembly.GetAssembly(GetType()));

            //Assert
            Assert.AreSame(includedOnly, result);
            Assert.IsTrue(result.Assemblies.Contains(Assembly.GetAssembly(GetType())));
        }

        [TestMethod]
        public void ShouldAddAssemblyToIncludedOnlyAssembliesUsingAndAssembly()
        {
            //Act
            var includedOnly = new IncludedOnlyAssemblies();
            var result = includedOnly.Assembly(Assembly.GetAssembly(typeof(System.DateTime))).
                               AndAssembly(Assembly.GetAssembly(typeof(Enumerable)));

            //Assert
            Assert.AreSame(includedOnly, result);
            Assert.IsTrue(result.Assemblies.Contains(Assembly.GetAssembly(typeof(System.DateTime))));
            Assert.IsTrue(result.Assemblies.Contains(Assembly.GetAssembly(typeof(Enumerable))));
        }

        [TestMethod]
        public void ShouldReturnIncludedAssemblies()
        {
            //Act
            var includedOnly = new IncludedOnlyAssemblies();
            var result = includedOnly.Including;

            //Assert
            Assert.AreSame(Bootstrapper.Including, result);
        }

        [TestMethod]
        public void ShouldReturnBootstrapperAssemblies()
        {
            //Arrange
            var includedOnly = new IncludedOnlyAssemblies();

            //Act
            var result = includedOnly.Assembly(Assembly.GetAssembly(typeof(System.DateTime)));

            //Assert
            Assert.IsTrue(result.Assemblies.Count(a => a.FullName.StartsWith("Bootstrapper")) > 1);
        }        

        [TestMethod]
        public void ShouldReturnExcludedAssemblies()
        {
            //Act
            var includedOnly = new IncludedOnlyAssemblies();
            var result = includedOnly.Excluding;

            //Assert
            Assert.AreSame(Bootstrapper.Excluding, result);
        }

        [TestMethod]
        public void ShouldReturnBootstrapperExtensions()
        {
            //Act
            var includedOnly = new IncludedOnlyAssemblies();
            var result = includedOnly.With;

            //Assert
            Assert.AreSame(Bootstrapper.With, result);
        }

        [TestMethod]
        public void ShouldInvokeBootstrapperStart()
        {
            //Act
            var includedOnly = new IncludedOnlyAssemblies();
            var extension = A.Fake<IBootstrapperExtension>();
            Bootstrapper.With.Extension(extension);

            //Act
            includedOnly.Start();
            Bootstrapper.ClearExtensions();

            //Assert
            A.CallTo(() => extension.Run()).MustHaveHappened();
        }
    }
}
