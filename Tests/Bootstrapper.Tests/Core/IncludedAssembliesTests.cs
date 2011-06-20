using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions;
using FakeItEasy;
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
        public void ShouldReturnExcludedAssemblies()
        {
            //Act
            var included = new IncludedAssemblies();
            var result = included.Excluding;

            //Assert
            Assert.AreSame(Bootstrapper.Excluding, result);
        }


        [TestMethod]
        public void ShouldReturnBootstrapperExtensions()
        {
            //Act
            var included = new IncludedAssemblies();
            var result = included.With;

            //Assert
            Assert.AreSame(Bootstrapper.With, result);
        }

        [TestMethod]
        public void ShouldInvokeBootstrapperStart()
        {
            //Act
            var included = new IncludedAssemblies();
            var extension = A.Fake<IBootstrapperExtension>();
            Bootstrapper.With.Extension(extension);

            //Act
            included.Start();
            Bootstrapper.ClearExtensions();

            //Assert
            A.CallTo(() => extension.Run()).MustHaveHappened();
        }
    }
}
