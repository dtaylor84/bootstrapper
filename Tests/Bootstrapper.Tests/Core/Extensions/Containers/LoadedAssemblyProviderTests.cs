using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bootstrap.Extensions.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.Containers
{
    [TestClass]
    public class LoadedAssemblyProviderTests
    {
        [TestMethod]
        public void Constructor_WhenInvoked_ShouldReturnLoadedAssemblyProvider()
        {
            //Act
            var result = new LoadedAssemblyProvider();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (IBootstrapperAssemblyProvider));
            Assert.IsInstanceOfType(result, typeof (LoadedAssemblyProvider));            
        }

        [TestMethod]
        public void GetAssemblies_WhenInvoked_ShouldReturnLoadedAssemblies()
        {
            //Act
            var result = new LoadedAssemblyProvider().GetAssemblies();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (IEnumerable<Assembly>));
            Assert.IsTrue(result.Any());            
        }
    }
}
