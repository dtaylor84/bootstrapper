using Bootstrap.Extensions;
using Bootstrap.NHibernate.Wcf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.NHibernateWcf
{
    [TestClass]
    public class NHibernateWcfExtensionTests
    {
        [TestMethod]
        public void ShouldCreateANewNHibernateExtension()
        {
            //Act
            var result = new NHibernateWcfExtension();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperExtension));
            Assert.IsInstanceOfType(result, typeof(NHibernateWcfExtension));
        }
    }
}
