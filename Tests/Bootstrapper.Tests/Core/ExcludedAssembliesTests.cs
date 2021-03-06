﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core
{
    [TestClass]
    public class ExcludedAssembliesTests
    {
        [TestMethod]
        public void ShouldCreateANewExcludedAssemblies()
        {
            //Act
            var result = new ExcludedAssemblies();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IExcludedAssemblies));
            Assert.IsInstanceOfType(result, typeof(ExcludedAssemblies));
            Assert.IsInstanceOfType(result, typeof(IBootstrapperOption));
            Assert.IsInstanceOfType(result, typeof(BootstrapperOption));
        }

        [TestMethod]
        public void ShouldExcludeDefaultSystemAssemblies()
        {
            //Act
            var result = new ExcludedAssemblies().Assemblies;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<string>));
            Assert.IsTrue(result.Contains("System"));
            Assert.IsTrue(result.Contains("mscorlib"));
            Assert.IsTrue(result.Contains("Microsoft.VisualStudio"));            
        }

        [TestMethod]
        public void ShouldAddAssemblyNameToExcludedAssemblies()
        {
            //Act
            var excluded = new ExcludedAssemblies();
            var result =  excluded.Assembly("Test");

            //Assert
            Assert.AreSame(excluded, result);
            Assert.IsTrue(result.Assemblies.Contains("Test"));
        }

        [TestMethod]
        public void ShouldAddAssemblyNameToExcludedAssembliesUsingAndAssembly()
        {
            //Act
            var excluded = new ExcludedAssemblies();
            var result = excluded.Assembly("Test1").AndAssembly("Test2");

            //Assert
            Assert.AreSame(excluded, result);
            Assert.IsTrue(result.Assemblies.Contains("Test1"));
            Assert.IsTrue(result.Assemblies.Contains("Test2"));
        }
    }
}
