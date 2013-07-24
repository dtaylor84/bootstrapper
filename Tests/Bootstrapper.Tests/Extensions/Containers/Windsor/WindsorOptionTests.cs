using Bootstrap.Extensions.Containers;
using Bootstrap.Windsor;
using Castle.Windsor;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Extensions.Containers.Windsor
{
    [TestClass]
    public class WindsorOptionTests
    {
        [TestMethod]
        public void Constructor_WhenInvoked_ShouldReturnWindsorOptions()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();

            //Act
            var result = new WindsorOptions(containerOptions);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WindsorOptions));
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
            Assert.IsInstanceOfType(result, typeof(BootstrapperOption));
            Assert.IsInstanceOfType(result, typeof(IBootstrapperOption));
        }

        [TestMethod]
        public void Constructor_WhenInvoked_ShouldNotInitializeTheContainer()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();

            //Act
            var result = new WindsorOptions(containerOptions);

            //Assert
            Assert.IsNull(result.Container);
        }

        [TestMethod]
        public void WithContainer_WhenInvoked_ShouldReturnTheSameWindsorOptions()
        {
            //Arrange
            var container = A.Fake<IWindsorContainer>();
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new WindsorOptions(containerOptions);

            //Act
            var result = options.WithContainer(container);

            //Assert
            Assert.AreSame(options, result);
        }

        [TestMethod]
        public void WithContainer_WhenInvoked_ShouldSetTheContainer()
        {
            //Arrange
            var container = A.Fake<IWindsorContainer>();
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new WindsorOptions(containerOptions);

            //Act
            options.WithContainer(container);

            //Assert
            Assert.AreSame(container, options.Container);
        }


        [TestMethod]
        public void AutoRegistration_WhenInvoked_ShouldReturnTheValueOfTheOptionsAutoRegistration()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new WindsorOptions(containerOptions);
            A.CallTo(() => containerOptions.AutoRegistration).Returns(true);

            //Act
            var result = options.AutoRegistration;

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UsingAutoRegistration_WhenInvoked_ShouldReturnTheSameWindsorOptions()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new WindsorOptions(containerOptions);

            //Act
            var result = options.UsingAutoRegistration();

            //Assert
            Assert.AreSame(options, result);
        }

        [TestMethod]
        public void UsingAutoRegistration_WhenInvoked_ShouldInvokeUsingAutoRegistrationInTheOptions()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new WindsorOptions(containerOptions);

            //Act
            options.UsingAutoRegistration();

            //Assert
            A.CallTo(() => containerOptions.UsingAutoRegistration()).MustHaveHappened();
        }
    }
}
