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
        public bool Reseted { get; set; }
        public Dictionary<Type, Type> Registrations { get; set; }

        public TestContainerExtension(IRegistrationHelper registrationHelper): base(registrationHelper)
        {
            RegistrationsRegistered = false;
            RegistrationsInvoked = false;
            Registrations = new Dictionary<Type, Type>();
        }

        public void SetTestServiceLocator(IServiceLocator theLocator){}
        public override void RegisterAll<TTarget>() {}
        public override void SetServiceLocator() {}
        public override void ResetServiceLocator() {}
        public override T Resolve<T>() {return null;}
        public override IList<T> ResolveAll<T>() {return null;}
        public override void Register<TTarget, TImplementation>() { Registrations.Add(typeof(TTarget), typeof(TImplementation));}
        public override void Register<TTarget>(TTarget implementation) {}
        public void DoAutoRegister() {AutoRegister();}

        protected override void ResetContainer() {Reseted=true;}
        protected override void InitializeContainer() { Container = new object();  }
        protected override void RegisterImplementationsOfIRegistration() {RegistrationsRegistered = true;}
        protected override void InvokeRegisterForImplementationsOfIRegistration(){RegistrationsInvoked = true;}

    }
}
