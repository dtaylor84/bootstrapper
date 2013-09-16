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
        public void Constructor_WhenInvoked_ShouldCreateATestContainerExtension()
        {
            //Act
            var result = new TestContainerExtension(registrationHelper);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TestContainerExtension));
        }

        [TestMethod]
        public void Constructor_WhenInvoked_ShouldAddMicrosoftPracticesToExcludedAssemblies()
        {
            //Act
            var result = new TestContainerExtension(registrationHelper);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Bootstrapper.Excluding.Assemblies.Contains("Microsoft.Practices"));
        }

        [TestMethod]
        public void Run_WhenInvoked_ShouldRegisterAndInvokeRegistrations()
        {
            //Arrange
            var containerExtension = new TestContainerExtension(registrationHelper);

            //Act
            containerExtension.Run();

            //Assert
            Assert.IsTrue(containerExtension.RegistrationsRegistered);
            Assert.IsTrue(containerExtension.RegistrationsInvoked);
        }

        [TestMethod]
        public void Reset_WhenInvoked_ShouldInvokeResetContainerInTheImplementationClass()
        {
            //Arrange
            var containerExtension = new TestContainerExtension(registrationHelper);

            //Act
            containerExtension.Reset();

            //Assert
            Assert.IsTrue(containerExtension.Reseted);
        }

        [TestMethod]
        public void AutoRegister_WhenInvoked_ShouldRegisterBasedOnConvention()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(RegisteredByConvention));
            A.CallTo(() => registrationHelper.GetAssemblies()).Returns(new List<Assembly> { assembly });
            var containerExtension = new TestContainerExtension(registrationHelper);

            //Act
            containerExtension.DoAutoRegister();

            //Assert
            var result = containerExtension.Registrations;
            A.CallTo(() => registrationHelper.GetAssemblies()).MustHaveHappened();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(Dictionary<Type,Type>));
            Assert.IsTrue(result.ContainsKey(typeof(IRegisteredByConvention)));
            Assert.AreSame(typeof(RegisteredByConvention), result[typeof(IRegisteredByConvention)]);
        }
    }
}
