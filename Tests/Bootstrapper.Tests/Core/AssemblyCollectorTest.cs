using System.Reflection;
using Bootstrap.Tests.Other;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;

namespace Bootstrap.Tests.Core
{
    [TestClass]
    public class AssemblyCollectorTest
    {

        [TestMethod]
        public void ShouldCreateAnAssemblyCollector()
        {
            //Arrange
            var extension = new Mock<IBootstrapperContainerExtension>();

            //Act
            var result = new AssemblyCollector(extension.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(AssemblyCollector));
        }

        [TestMethod]
        public void ShouldReturnAReadOnlyIListOfAssemblyNames()
        {
            //Arrange
            var extension = new Mock<IBootstrapperContainerExtension>();
            var collector = new AssemblyCollector(extension.Object);

            //Act
            var result = collector.AssemblyNames;
            
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            ExceptionAssert.Throws<NotSupportedException>(() => result.Add("test"));
        }

        [TestMethod]
        public void ShouldAddAssemblyName()
        {
            //Arrange
            var extension = new Mock<IBootstrapperContainerExtension>();
            var collector = new AssemblyCollector(extension.Object);
            const string assemblyName1 = "test1";
            const string assemblyName2 = "test2";

            //Act
            collector.InAssemblyNamed(assemblyName1);
            collector.InAssemblyNamed(assemblyName2);
            var result = collector.AssemblyNames;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(assemblyName1, result[0]);
            Assert.AreEqual(assemblyName2, result[1]);
        }

        [TestMethod]
        public void ShouldNotAddDuplicateAssemblyNames()
        {
            //Arrange
            var extension = new Mock<IBootstrapperContainerExtension>();
            var collector = new AssemblyCollector(extension.Object);
            const string assemblyName1 = "test1";
            const string assemblyName2 = "test1";

            //Act
            collector.InAssemblyNamed(assemblyName1);
            collector.InAssemblyNamed(assemblyName2);
            var result = collector.AssemblyNames;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(assemblyName1, result[0]);
        }

        [TestMethod]
        public void ShouldReturnAReadOnlyIListOfAssemblies()
        {
            //Arrange
            var extension = new Mock<IBootstrapperContainerExtension>();
            var collector = new AssemblyCollector(extension.Object);

            //Act
            var result = collector.Assemblies;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            ExceptionAssert.Throws<NotSupportedException>(() => result.Add(Assembly.GetExecutingAssembly()));
        }

        [TestMethod]
        public void ShouldAddAssembly()
        {
            //Arrange
            var extension = new Mock<IBootstrapperContainerExtension>();
            var collector = new AssemblyCollector(extension.Object);
            var assembly1 = Assembly.GetExecutingAssembly();
            var assembly2 = Assembly.GetAssembly(typeof(Bootstrap.Bootstrapper));

            //Act
            collector.InAssembly(assembly1);
            collector.InAssembly(assembly2);
            var result = collector.Assemblies;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(assembly1, result[0]);
            Assert.AreEqual(assembly2, result[1]);
        }

        [TestMethod]
        public void ShouldNotAddDuplicateAssemblies()
        {
            //Arrange
            var extension = new Mock<IBootstrapperContainerExtension>();
            var collector = new AssemblyCollector(extension.Object);
            var assembly1 = Assembly.GetExecutingAssembly();
            var assembly2 = Assembly.GetExecutingAssembly();

            //Act
            collector.InAssembly(assembly1);
            collector.InAssembly(assembly2);
            var result = collector.Assemblies;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(assembly1, result[0]);
        }

        [TestMethod]
        public void ShouldNotAddNullAssemblies()
        {
            //Arrange
            var extension = new Mock<IBootstrapperContainerExtension>();
            var collector = new AssemblyCollector(extension.Object);
            var assembly1 = Assembly.GetEntryAssembly();

            //Act
            collector.InAssembly(assembly1);
            var result = collector.Assemblies;

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldReturnBootstrapperContainerExtension()
        {
            //Arrange
            var extension = new Mock<IBootstrapperContainerExtension>();
            var collector = new AssemblyCollector(extension.Object);

            //Act
            var result = collector.InAssemblyNamed(string.Empty);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtension));
            Assert.AreSame(extension.Object, result);
        }

    }
}
