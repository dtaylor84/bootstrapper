using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Bootstrap.AutoMapper;
using Bootstrap.Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core
{
    [TestClass]
    public class IncludedAssembliesTests
    {
        [TestMethod]
        public void ShouldCreateANewIncludedAssemblies()
        {
            //Act
            var result = new IncludedAssemblies();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IIncludedAssemblies));
            Assert.IsInstanceOfType(result, typeof(IncludedAssemblies));
            Assert.IsInstanceOfType(result, typeof(IBootstrapperOption));
            Assert.IsInstanceOfType(result, typeof(BootstrapperOption));
        }

        [TestMethod]
        public void ShouldReturnEmpyListOfAssemblies()
        {
            //Act
            var result = new IncludedAssemblies().Assemblies;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Assembly>));
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void ShouldAddAssemblyToIncludedAssemblies()
        {
            //Act
            var included = new IncludedAssemblies();
            var result = included.Assembly(Assembly.GetAssembly(GetType()));

            //Assert
            Assert.AreSame(included, result);
            Assert.IsTrue(result.Assemblies.Contains(Assembly.GetAssembly(GetType())));
        }

        [TestMethod]
        public void ShouldAddAssemblyRangeToIncludedAssemblies()
        {
            //Act
            var included = new IncludedAssemblies();
            var assemblyRange = new List<Assembly>
                                    {
                                        Assembly.GetAssembly(typeof (Bootstrapper)),
                                        Assembly.GetAssembly(typeof (AutoMapperExtension))
                                    };
            var result = included.AssemblyRange(assemblyRange);

            //Assert
            Assert.AreSame(included, result);
            Assert.IsTrue(result.Assemblies.Contains(Assembly.GetAssembly(typeof(Bootstrapper))));
            Assert.IsTrue(result.Assemblies.Contains(Assembly.GetAssembly(typeof(AutoMapperExtension))));
        }

        [TestMethod]
        public void ShouldAddAssemblyToIncludedAssembliesUsingAndAssembly()
        {
            //Act
            var included = new IncludedAssemblies();
            var result = included.Assembly(Assembly.GetAssembly(typeof(Bootstrapper))).
                               AndAssembly(Assembly.GetAssembly(typeof(AutoMapperExtension)));

            //Assert
            Assert.AreSame(included, result);
            Assert.IsTrue(result.Assemblies.Contains(Assembly.GetAssembly(typeof(Bootstrapper))));
            Assert.IsTrue(result.Assemblies.Contains(Assembly.GetAssembly(typeof(AutoMapperExtension))));
        }

        [TestMethod]
        public void ShouldAddAssemblyRangeToIncludedAssembliesUsingAndAssemblyRange()
        {
            //Act
            var included = new IncludedAssemblies();
            var assemblyRange = new List<Assembly>
                                    {
                                        Assembly.GetAssembly(typeof (AutofacExtension)),
                                        Assembly.GetAssembly(typeof (AutoMapperExtension))
                                    };
            var result = included.Assembly(Assembly.GetAssembly(typeof(Bootstrapper))).
                               AndAssemblyRange(assemblyRange);

            //Assert
            Assert.AreSame(included, result);
            Assert.IsTrue(result.Assemblies.Contains(Assembly.GetAssembly(typeof(Bootstrapper))));
            Assert.IsTrue(result.Assemblies.Contains(Assembly.GetAssembly(typeof(AutoMapperExtension))));
            Assert.IsTrue(result.Assemblies.Contains(Assembly.GetAssembly(typeof (AutofacExtension))));
        }
    }
}
