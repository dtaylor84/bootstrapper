using System;
using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions.Containers;
using Castle.Facilities.FactorySupport;
using Castle.Windsor;
using Castle.Core;
using Castle.MicroKernel.Registration;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;

namespace Bootstrap.Windsor
{
    public class WindsorExtension: BootstrapperContainerExtension
    {
        private IWindsorContainer container;

        protected override void InitializeContainer()
        {
            container = new WindsorContainer()
                .AddFacility<FactorySupportFacility>();
            Container = container;
        }

        protected override void RegisterImplementationsOfIRegistration()
        {
            RegisterAll<IBootstrapperRegistration>();
            RegisterAll<IWindsorRegistration>();
        }

        protected override void InvokeRegisterForImplementationsOfIRegistration()
        {
            container.ResolveAll<IBootstrapperRegistration>().ForEach(r => r.Register(this));
            container.ResolveAll<IWindsorRegistration>().ForEach(r => r.Register(container));
        }

        public override void RegisterAll<TTarget>()
        {
            AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToList().ForEach(
                a => container.Register(AllTypes.FromAssembly(a).BasedOn<TTarget>().WithService.Base()));
        }

        public override void SetServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }

        public override void ResetServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => null);
        }

        public override T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public override IList<T> ResolveAll<T>()
        {
            return container.ResolveAll<T>();
        }

        public override void Register<TTarget, TImplementation>()
        {
            container.Register(Component.For<TTarget>().ImplementedBy<TImplementation>());

        }

        public override void Register<TTarget>(TTarget implementation)
        {
            container.Register(Component.For<TTarget>().Instance(implementation));
        }

        protected override void ResetContainer()
        {
            container = null;
            Container = null;
        }
    }
}
