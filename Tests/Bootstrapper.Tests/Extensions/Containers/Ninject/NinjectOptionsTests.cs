using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Bootstrap.Ninject;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace Bootstrap.Tests.Extensions.Containers.Ninject
{
    [TestClass]
    public class NinjectOptionsTests
    {
        [TestMethod]
        public void Constructor_WhenInvoked_ShouldReturnNinjectOptions()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();

            //Act
            var result = new NinjectOptions(containerOptions);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NinjectOptions));
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
        }

        [TestMethod]
        public void Constructor_WhenInvoked_ShouldNotInitializeTheContainer()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();

            //Act
            var result = new NinjectOptions(containerOptions);

            //Assert
            Assert.IsNull(result.Container);
        }

        [TestMethod]
        public void WithContainer_WhenInvoked_ShouldReturnTheSameNinjectOptions()
        {
            //Arrange
            var container = A.Fake<IKernel>();
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new NinjectOptions(containerOptions);

            //Act
            var result = options.WithContainer(container);

            //Assert
            Assert.AreSame(options, result);
        }

        [TestMethod]
        public void WithContainer_WhenInvoked_ShouldSetTheContainer()
        {
            //Arrange
            var container = A.Fake<IKernel>();
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new NinjectOptions(containerOptions);

            //Act
            options.WithContainer(container);

            //Assert
            Assert.AreSame(container, options.Container);
        }

        [TestMethod]
        public void And_WhenInvoked_ShouldReturnTheValueOfTheOptionsAnd()
        {
            //Arrange
            var extensions = new BootstrapperExtensions();
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new NinjectOptions(containerOptions);
            A.CallTo(() => containerOptions.And).Returns(extensions);

            //Act
            var result = options.And;

            //Assert
            Assert.AreSame(extensions, result);
        }

        [TestMethod]
        public void AutoRegistration_WhenInvoked_ShouldReturnTheValueOfTheOptionsAutoRegistration()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new NinjectOptions(containerOptions);
            A.CallTo(() => containerOptions.AutoRegistration).Returns(true);

            //Act
            var result = options.AutoRegistration;

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UsingAutoRegistration_WhenInvoked_ShouldReturnTheSameNinjectOptions()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new NinjectOptions(containerOptions);

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
            var options = new NinjectOptions(containerOptions);

            //Act
            options.UsingAutoRegistration();

            //Assert
            A.CallTo(() => containerOptions.UsingAutoRegistration()).MustHaveHappened();
        }

        [TestMethod]
        public void Start_WhenInvoked_ShouldInvokeStartInTheOptions()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new NinjectOptions(containerOptions);

            //Act
            options.Start();

            //Assert
            A.CallTo(() => containerOptions.Start()).MustHaveHappened();
        }
    }
}
