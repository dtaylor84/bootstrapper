using Bootstrap.Extensions.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core
{
    [TestClass]
    public class AssemblySetOptionsTests
    {
        [TestMethod]
        public void Constructor_WhenInvoked_ShouldReturnAssemblySetOptions()
        {
            //Act
            var result = new AssemblySetOptions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (IAssemblySetOptions));
            Assert.IsInstanceOfType(result, typeof (IBootstrapperOption));
            Assert.IsInstanceOfType(result, typeof (BootstrapperOption));
            Assert.IsInstanceOfType(result, typeof (AssemblySetOptions));            
        }

        [TestMethod]
        public void LoadedAssemblies_WhenInvoked_ShouldSetTheBootstrapperRegistrationHelperToANewRegistrationHelperWithALoadedAssemblyProvider()
        {           
            //Act
            new AssemblySetOptions().LoadedAssemblies();
            var result = Bootstrapper.RegistrationHelper as RegistrationHelper;

            //Assert
            Assert.IsNotNull(result);            
            Assert.IsTrue(result.AssemblyProvider is LoadedAssemblyProvider);
        }

        [TestMethod]
        public void ReferencedAssemblies_WhenInvoked_ShouldSetTheBootstrapperRegistrationHelperToANewRegistrationHelperWithAReferencedAssemblyProvider()
        {           
            //Act
            new AssemblySetOptions().ReferencedAssemblies();
            var result = Bootstrapper.RegistrationHelper as RegistrationHelper;

            //Assert
            Assert.IsNotNull(result);            
            Assert.IsTrue(result.AssemblyProvider is ReferencedAssemblyProvider);
        }
    }
}
