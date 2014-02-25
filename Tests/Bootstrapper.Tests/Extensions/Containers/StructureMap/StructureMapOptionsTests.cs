using Bootstrap.Extensions.Containers;
using Bootstrap.StructureMap;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace Bootstrap.Tests.Extensions.Containers.StructureMap
{
    [TestClass]
    public class StructureMapOptionsTests
    {
        [TestMethod]
        public void Constructor_WhenInvoked_ShouldReturnStructureMapOptions()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();

            //Act
            var result = new StructureMapOptions(containerOptions);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StructureMapOptions));
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtensionOptions));
        }

        [TestMethod]
        public void Constructor_WhenInvoked_ShouldNotInitializeTheContainer()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();

            //Act
            var result = new StructureMapOptions(containerOptions);

            //Assert
            Assert.IsNull(result.Container);
        }

        [TestMethod]
        public void WithContainer_WhenInvoked_ShouldReturnTheSameStructureMapOptions()
        {
            //Arrange
            var container = A.Fake<IContainer>();
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new StructureMapOptions(containerOptions);

            //Act
            var result = options.WithContainer(container);

            //Assert
            Assert.AreSame(options, result);
        }

        [TestMethod]
        public void WithContainer_WhenInvoked_ShouldSetTheContainer()
        {
            //Arrange
            var container = A.Fake<IContainer>();
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new StructureMapOptions(containerOptions);

            //Act
            options.WithContainer(container);

            //Assert
            Assert.AreSame(container, options.Container);
        }

        [TestMethod]
        public void UsingObjectFactory_WhenInvoked_ShouldReturnTheSameStructureMapOptions()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new StructureMapOptions(containerOptions);

            //Act
            var result = options.UsingObjectFactory();

            //Assert
            Assert.AreSame(options, result);
        }

        [TestMethod]
        public void UsingObjectFactory_WhenInvoked_ShouldSetTheContainerToTheObjectFactoryContainer()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new StructureMapOptions(containerOptions);

            //Act
            options.UsingObjectFactory();

            //Assert
            Assert.AreSame(ObjectFactory.Container, options.Container);
        }

        [TestMethod]
        public void AutoRegistration_WhenInvoked_ShouldReturnTheValueOfTheOptionsAutoRegistration()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new StructureMapOptions(containerOptions);
            A.CallTo(() => containerOptions.AutoRegistration).Returns(true);

            //Act
            var result = options.AutoRegistration;

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UsingAutoRegistration_WhenInvoked_ShouldReturnTheSameStructureMapOptions()
        {
            //Arrange
            var containerOptions = A.Fake<IBootstrapperContainerExtensionOptions>();
            var options = new StructureMapOptions(containerOptions);

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
            var options = new StructureMapOptions(containerOptions);

            //Act
            options.UsingAutoRegistration();

            //Assert
            A.CallTo(() => containerOptions.UsingAutoRegistration()).MustHaveHappened();
        }
    }
}
