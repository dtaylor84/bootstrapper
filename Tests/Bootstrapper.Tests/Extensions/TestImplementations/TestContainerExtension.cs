using System;
using System.Collections.Generic;
using Bootstrap.Extensions.Containers;
using Microsoft.Practices.ServiceLocation;

namespace Bootstrap.Tests.Extensions.TestImplementations
{
    public class TestContainerExtension: BootstrapperContainerExtension
    {
        public bool RegistrationsRegistered { get; set; }
        public bool RegistrationsInvoked { get; set; }

        public TestContainerExtension()
        {
            RegistrationsRegistered = false;
            RegistrationsInvoked = false;
        }

        public void SetTestServiceLocator(IServiceLocator theLocator)
        {
            throw new NotImplementedException();
        }

        public override void RegisterAll<TTarget>()
        {
            throw new NotImplementedException();
        }

        public override void SetServiceLocator()
        {
            throw new NotImplementedException();
        }

        public override void ResetServiceLocator()
        {
            throw new NotImplementedException();
        }

        public override T Resolve<T>()
        {
            throw new NotImplementedException();
        }

        public override IList<T> ResolveAll<T>()
        {
            throw new NotImplementedException();
        }

        public override void Register<TTarget, TImplementation>()
        {
            throw new NotImplementedException();
        }

        public override void Register<TTarget>(TTarget implementation)
        {
            throw new NotImplementedException();
        }

        protected override void ResetContainer() {Container = null;}
        protected override void InitializeContainer() { Container = new object(); }
        protected override void RegisterImplementationsOfIRegistration() {RegistrationsRegistered = true;}
        protected override void InvokeRegisterForImplementationsOfIRegistration(){RegistrationsInvoked = true;}
    }
}
