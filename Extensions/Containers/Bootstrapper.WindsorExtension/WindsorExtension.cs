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
        public IBootstrapperContainerExtensionOptions Options { get; private set; }

        public WindsorExtension(IRegistrationHelper registrationHelper): base(registrationHelper)
        {
            Options= new BootstrapperContainerExtensionOptions();
            Bootstrapper.Excluding.Assembly("Castle");
        }

        public void InitializeContainer(IWindsorContainer aContainer)
        {
            container = aContainer;
            Container = container;
        }

        protected override void InitializeContainer()
        {
            container = new WindsorContainer()
                .AddFacility<FactorySupportFacility>();
            Container = container;
        }

        protected override void RegisterImplementationsOfIRegistration()
        {
            CheckContainer();
            if(Options.AutoRegistration) AutoRegister();
            RegisterAll<IBootstrapperRegistration>();
            RegisterAll<IWindsorRegistration>();
        }

        protected override void InvokeRegisterForImplementationsOfIRegistration()
        {
            CheckContainer();
            container.ResolveAll<IBootstrapperRegistration>().ForEach(r => r.Register(this));
            container.ResolveAll<IWindsorRegistration>().ForEach(r => r.Register(container));
        }

        public override void SetServiceLocator()
        {
            CheckContainer();
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }

        public override void ResetServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => null);
        }

        public override T Resolve<T>()
        {
            CheckContainer();
            return container.Resolve<T>();
        }

        public override IList<T> ResolveAll<T>()
        {
            CheckContainer();
            return container.ResolveAll<T>();
        }

        public override void Register<TTarget, TImplementation>()
        {
            CheckContainer();
            if (!container.Kernel.HasComponent(typeof(TTarget).Name) ||
                container.Resolve<TTarget>().GetType() != typeof(TImplementation))
                container.Register(Component.For<TTarget>().ImplementedBy<TImplementation>());
        }

        public override void Register<TTarget>(TTarget implementation)
        {
            CheckContainer();
            if (!container.Kernel.HasComponent(typeof(TTarget).Name) ||
                container.Resolve<TTarget>().GetType() != implementation.GetType())
                container.Register(Component.For<TTarget>().Instance(implementation));
        }

        public override void RegisterAll<TTarget>()
        {
            CheckContainer();
            RegistrationHelper.GetAssemblies().ToList().ForEach(
                a => container.Register(AllTypes.FromAssembly(a).BasedOn<TTarget>().WithService.Base()));
        }

        protected override void ResetContainer()
        {
            container = null;
            Container = null;
        }
    }
}
