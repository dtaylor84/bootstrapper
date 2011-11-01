using System;
using System.Collections.Generic;
using System.Reflection;
using Bootstrap.Extensions.Containers;
using Bootstrap.Tests.Extensions.TestImplementations;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.Containers
{
    [TestClass]
    public class BootstrapperContainerExtensionTests
    {
        private IRegistrationHelper registrationHelper;

        [TestInitialize]
        public void Initialize()
        {
            registrationHelper = A.Fake<IRegistrationHelper>();
        }

        [TestMethod]
        public void ShouldCreateATestContainerExtension()
        {
            //Act
            var result = new TestContainerExtension(registrationHelper);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TestContainerExtension));
        }

        [TestMethod]
        public void ShouldAddMicrosoftPracticesToExcludedAssemblies()
        {
            //Act
            var result = new TestContainerExtension(registrationHelper);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Bootstrapper.Excluding.Assemblies.Contains("Microsoft.Practices"));
        }

        [TestMethod]
        public void ShouldSetTheBootstrapperContainer()
        {
            //Arrange
            var containerExtension = new TestContainerExtension(registrationHelper);

            //Act
            Bootstrapper.With.Extension(containerExtension).Start();
            var result = Bootstrapper.Container;
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ShouldRegisterAndInvokeRegistrations()
        {
            //Arrange
            var containerExtension = new TestContainerExtension(registrationHelper);

            //Act
            Bootstrapper.With.Extension(containerExtension).Start();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsTrue(containerExtension.RegistrationsRegistered);
            Assert.IsTrue(containerExtension.RegistrationsInvoked);
        }

        [TestMethod]
        public void ShouldResetTheBootstrapperContainer()
        {
            //Arrange
            var containerExtension = new TestContainerExtension(registrationHelper);
            Bootstrapper.With.Extension(containerExtension).Start();

            //Act
            containerExtension.Reset();
            var result = Bootstrapper.Container;
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldRegisterBasedOnConvention()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(RegisteredByConvention));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });
            var containerExtension = new TestContainerExtension(registrationHelper);

            //Act
            containerExtension.DoAutoRegister();
            var result = containerExtension.Registrations;

            //Assert
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(Dictionary<Type,Type>));
            Assert.IsTrue(result.ContainsKey(typeof(IRegisteredByConvention)));
            Assert.AreSame(typeof(RegisteredByConvention), result[typeof(IRegisteredByConvention)]);
        }
    }
}
