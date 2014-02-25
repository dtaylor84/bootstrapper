using Bootstrap.Extensions.Containers;
using Bootstrap.Tests.Extensions.TestImplementations;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.Containers
{
    [TestClass]
    public class RegistrationInvokerTests
    {
        [TestMethod]
        public void ShouldCreateANewRegistrationInvoker()
        {
            //Arrange
            var containerExtension = A.Fake<IBootstrapperContainerExtension>();

            //Act
            var result = new RegistrationInvoker<IRegisteredByConvention, RegisteredByConvention>(containerExtension);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IRegistrationInvoker<IRegisteredByConvention, RegisteredByConvention>));
            Assert.IsInstanceOfType(result, typeof(RegistrationInvoker<IRegisteredByConvention, RegisteredByConvention>));
        }

        [TestMethod]
        public void ShouldInvokeRegisterInContainerExtension()
        {
            //Arrange
            var containerExtension = A.Fake<IBootstrapperContainerExtension>();
            var invoker = new RegistrationInvoker<IRegisteredByConvention, RegisteredByConvention>(containerExtension);

            //Act
            invoker.Register();

            //Assert
            A.CallTo(() => containerExtension.Register<IRegisteredByConvention,RegisteredByConvention>()).MustHaveHappened();            
        }

    }
}
