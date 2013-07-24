using System.Reflection;
using AutoMapper;
using Bootstrap.AutoMapper;
using Bootstrap.Autofac;
using Bootstrap.Locator;
using Bootstrap.Ninject;
using Bootstrap.SimpleInjector;
using Bootstrap.StructureMap;
using Bootstrap.Unity;
using Bootstrap.Windsor;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Adapters
{
    [TestClass]
    public class ServiceLocatorAdapterTests
    {
        private Assembly assembly;

        [TestInitialize]
        public void Initialize()
        {
            Bootstrapper.ClearExtensions();
            ServiceLocator.SetLocatorProvider(() => null);
            assembly = Assembly.GetAssembly(typeof (AutoMapperRegistration));
        }

        [TestMethod]
        public void WindsorAdapter_WhenUsed_ShouldAccessTheUnderlyingContainer()
        {
            //Act
            Bootstrapper
                .IncludingOnly.Assembly(assembly)
                .With.Windsor()
                .And.ServiceLocator()
                .Start();   

            //Assert
            Assert.IsNotNull(Bootstrapper.ContainerExtension.Resolve<IProfileExpression>());
        }

        [TestMethod]
        public void UnityAdapter_WhenUsed_ShouldAccessTheUnderlyingContainer()
        {
            //Act
            Bootstrapper
                .IncludingOnly.Assembly(assembly)
                .With.Unity()
                .And.ServiceLocator()
                .Start();

            //Assert
            Assert.IsNotNull(Bootstrapper.ContainerExtension.Resolve<IProfileExpression>());
        }

        [TestMethod]
        public void StructureMapAdapter_WhenUsed_ShouldAccessTheUnderlyingContainer()
        {
            //Act
            Bootstrapper
                .IncludingOnly.Assembly(assembly)
                .With.StructureMap()
                .And.ServiceLocator()
                .Start();

            //Assert
            Assert.IsNotNull(Bootstrapper.ContainerExtension.Resolve<IProfileExpression>());
        }

        [TestMethod]
        public void SimpleInjectorAdapter_WhenUsed_ShouldAccessTheUnderlyingContainer()
        {
            //Act
            Bootstrapper
                .Excluding.Assembly("AutoMapper")
                .IncludingOnly.Assembly(assembly)
                .With.SimpleInjector()
                .And.ServiceLocator()
                .Start();

            //Assert
            Assert.IsNotNull(Bootstrapper.ContainerExtension.Resolve<IProfileExpression>());
        }

        [TestMethod]
        public void NinjectAdapter_WhenUsed_ShouldAccessTheUnderlyingContainer()
        {
            //Act
            Bootstrapper
                .IncludingOnly.Assembly(assembly)
                .With.Ninject()
                .And.ServiceLocator()
                .Start();

            //Assert
            Assert.IsNotNull(Bootstrapper.ContainerExtension.Resolve<IProfileExpression>());
        }

        [TestMethod]
        public void AutofacAdapter_WhenUsed_ShouldAccessTheUnderlyingContainer()
        {
            //Act
            Bootstrapper
                .IncludingOnly.Assembly(assembly)
                .With.Autofac()
                .And.ServiceLocator()
                .Start();

            //Assert
            Assert.IsNotNull(Bootstrapper.ContainerExtension.Resolve<IProfileExpression>());
        }


    }
}
