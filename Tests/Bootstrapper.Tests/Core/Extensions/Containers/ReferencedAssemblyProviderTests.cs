using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bootstrap.Extensions.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.Containers
{
    [TestClass]
    public class ReferencedAssemblyProviderTests
    {
        [TestMethod]
        public void Constructor_WhenInvoked_ShouldReturnReferencedAssemblyProvider()
        {
            //Act
            var result = new ReferencedAssemblyProvider();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (IBootstrapperAssemblyProvider));
            Assert.IsInstanceOfType(result, typeof (ReferencedAssemblyProvider));
        }

        [Ignore] //Untestable
        [TestMethod]
        public void GetAssemblies_WhenInvoked_ShouldReturnLoadedAssemblies()
        {
            //Act
            var result = new ReferencedAssemblyProvider().GetAssemblies();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Assembly>));
            Assert.IsTrue(result.Any());
        }
    }
}
