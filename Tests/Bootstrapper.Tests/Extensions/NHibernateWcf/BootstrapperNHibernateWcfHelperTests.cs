using Bootstrap.Extensions;
using Bootstrap.NHibernate.Wcf;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.NHibernateWcf
{
    [TestClass]
    public class BootstrapperNHibernateWcfHelperTests
    {
        [TestInitialize]
        [TestCleanup]
        public void Initialize()
        {
            Bootstrapper.ClearExtensions();
        }

        [TestMethod]
        public void ShouldAddTheNHibernateWcfExtensionToBootstrapper()
        {
            //Arrange
            Bootstrapper.ClearExtensions();
            var extension = A.Fake<IBootstrapperExtension>();

            //Act
            Bootstrapper.With.Extension(extension).And.NHibernateWcf();

            //Assert
            Assert.IsInstanceOfType(Bootstrapper.GetExtensions()[1], typeof(NHibernateWcfExtension));
        }
    }
}
