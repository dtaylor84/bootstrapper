using System.Collections;
using System.Linq;
using System.Reflection;
using Bootstrap.Tests.Adapters.Components;
using Bootstrap.Tests.Other;
using CommonServiceLocator.StructureMapAdapter;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace Bootstrap.Tests.Adapters
{
    [TestClass]
    public class StructureMapAdapterTests
    {
        private IServiceLocator locator;

        [TestInitialize]
        public void Initialize()
        {
            var container = new Container();
            container.Configure(c =>
                                c.Scan(s =>
                                           {
                                               s.AddAllTypesOf<ILogger>();
                                               s.Assembly(Assembly.GetAssembly(typeof(ILogger)));
                                           }));
            locator = new StructureMapServiceLocator(container);
        }

        [TestMethod]
        public void GetInstance_WhenInvokedForARegisteredType_ShouldReturnAnInstanceOfTheType()
        {
            //Act
            var result = locator.GetInstance<SimpleLogger>();

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
            var result1 = locator.GetInstance<SimpleLogger>();
            var result2 = locator.GetInstance(typeof(SimpleLogger), null);

            //Assert
            Assert.AreEqual(result1.GetType(), result2.GetType());
        }

        [TestMethod]
        public void GetAllInstances_WhenInvokedGenericallyOrNot_ShouldReturnTheSameResults()
        {
            //Act
            var result1 = locator.GetAllInstances<ILogger>().ToList();
            var result2 = locator.GetAllInstances(typeof(ILogger)).ToList();

            //Assert
            Assert.AreEqual(result1.Count(), result2.Count());
            Assert.IsFalse(result1.Any(r1 => result2.All(r2 => r2.GetType() != r1.GetType())));
        }

    }
}
