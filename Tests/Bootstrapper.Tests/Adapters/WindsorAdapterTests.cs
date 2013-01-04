using System.Collections;
using System.Linq;
using Bootstrap.Tests.Adapters.Components;
using Bootstrap.Tests.Other;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Adapters
{
    [TestClass]
    public class WindsorAdapterTests
    {
        private IServiceLocator locator;

        [TestInitialize]
        public void Initialize()
        {
            var container = new WindsorContainer()
                .Register(AllTypes.FromAssembly(typeof (ILogger).Assembly)
                                  .BasedOn<ILogger>()
                                  .WithService.FirstInterface()
                );
            locator = new WindsorServiceLocator(container);            
        }

        [TestMethod]
        public void GetInstance_WhenInvokedForARegisteredType_ShouldReturnAnInstanceOfTheType()
        {
            //Act
            var result = locator.GetInstance<ILogger>();

            //Assert
            Assert.IsNotNull(result, "instance should not be null");
        }

        [TestMethod]
        public void GetInstance_WhenInvokedForAnInvalidType_ShouldThrowAnActivationException()
        {
            //Act/Assert
            ExceptionAssert.Throws<ActivationException>(() => locator.GetInstance<IDictionary>());
        }

        [TestMethod]
        public void GetInstance_WhenInvokedWithARegisteredName_ShouldReturnAnInstanceOfTheType()
        {
            //Act
            var result = locator.GetInstance<ILogger>(typeof (AdvancedLogger).FullName);

            //Assert
            Assert.IsInstanceOfType(result, typeof (AdvancedLogger));            
        }

        [TestMethod]
        public void GetInstance_WhenInvokedWithAnInvalidName_ShouldThrowAnActivationException()
        {
            //Act/Assert
            ExceptionAssert.Throws<ActivationException>(() => locator.GetInstance<ILogger>(""));
        }

        [TestMethod]
        public void GetAllInstances_WhenInvokedWithARegisteredType_ShouldReturnAllInstances()
        {
            //Act
            var result = locator.GetAllInstances<ILogger>();

            //Assert
            Assert.AreEqual(2, result.Count());            
        }

        [TestMethod]
        public void GetAllInstances_WhenInvokedForAnInvalidType_ShouldReturnAnEmptyIEnumerable()
        {
            //Act
            var result = locator.GetAllInstances<IDictionary>();

            //Assert
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetInstance_WhenInvokedGenericallyOrNot_ShouldReturnTheSameResults()
        {
            //Act
            var result1 = locator.GetInstance<ILogger>();
            var result2 = locator.GetInstance(typeof (ILogger), null);

            //Assert
            Assert.AreEqual(result1.GetType(), result2.GetType());
        }

        [TestMethod]
        public void GetInstance_WhenInvokedWithNAmeGenricallyOrNot_ShouldReturnTheSameResults()
        {
            //Act
            var result1 = locator.GetInstance<ILogger>(typeof (AdvancedLogger).FullName);
            var result2 = locator.GetInstance(typeof (ILogger), typeof (AdvancedLogger).FullName);

            //Assert
            Assert.AreEqual(result1.GetType(), result2.GetType());
        }

        [TestMethod]
        public void GetInstance_WhenInvokedWithNoNameOrNullName_ShouldReturnTheSameResult()
        {
            //Act
            var result1 = locator.GetInstance<ILogger>();
            var result2 = locator.GetInstance<ILogger>(null);

            //Assert
            Assert.AreEqual(result1.GetType(), result2.GetType());
        }

        [TestMethod]
        public void GetAllInstances_WhenInvokedGenericallyOrNot_ShouldReturnTheSameResults()
        {
            //Act
            var result1 = locator.GetAllInstances<ILogger>().ToList();
            var result2 = locator.GetAllInstances(typeof (ILogger)).ToList();

            //Assert
            Assert.AreEqual(result1.Count(), result2.Count());
            Assert.IsFalse(result1.Any(r1 => !result2.Any(r2 => r2.GetType() == r1.GetType())));
        }
    }
}
