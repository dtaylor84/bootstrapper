using Bootstrap.Extensions.Containers;
using Bootstrap.SimpleInjector;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Bootstrap.Tests.Extensions.Containers.SimpleInjector
{
    [TestClass]
    public class SimpleInjectorOptionsTests
    {
        [TestMethod]
        public void Constructor_WhenInvoked_ShouldReturnSimpleInjectorOptions()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();

            //Act
            var result = new SimpleInjectorOptions(containerOptions);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (SimpleInjectorOptions));
            Assert.IsInstanceOfType(result, typeof (IBootstrapperContainerExtensionOptions));
        }

        [TestMethod]
        public void Constructor_WhenInvoked_ShouldNotInitializeTheContainer()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();

            //Act
            var result = new SimpleInjectorOptions(containerOptions);

            //Assert
            Assert.IsNull(result.Container);
        }

        [TestMethod]
        public void WithContainer_WhenInvoked_ShouldReturnTheSameSimpleInjectorOptions()
        {
            //Arrange
            var container = A.Fake<Container>();
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new SimpleInjectorOptions(containerOptions);

            //Act
            var result = options.WithContainer(container);

            //Assert
            Assert.AreSame(options, result);
        }

        [TestMethod]
        public void WithContainer_WhenInvoked_ShouldSetTheContainer()
        {
            //Arrange
            var container = A.Fake<Container>();
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new SimpleInjectorOptions(containerOptions);

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
            var options = new SimpleInjectorOptions(containerOptions);
            A.CallTo(() => containerOptions.AutoRegistration).Returns(true);

            //Act
            var result = options.AutoRegistration;

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UsingAutoRegistration_WhenInvoked_ShouldReturnTheSameSimpleInjectorOptions()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new SimpleInjectorOptions(containerOptions);

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
            var options = new SimpleInjectorOptions(containerOptions);

            //Act
            options.UsingAutoRegistration();

            //Assert
            A.CallTo(() => containerOptions.UsingAutoRegistration()).MustHaveHappened();
        }
    }
}
