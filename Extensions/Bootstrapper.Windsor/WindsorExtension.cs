using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions.Containers;
using Castle.Facilities.FactorySupport;
using Castle.MicroKernel;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using CommonServiceLocator.WindsorAdapter.Unofficial;
using Microsoft.Practices.ServiceLocation;

namespace Bootstrap.Windsor
{
    public class WindsorExtension: BootstrapperContainerExtension
    {
        private IWindsorContainer container;
        private readonly ICollection<IFacility> facilities;

        public WindsorOptions Options { get; private set; }

        public WindsorExtension(IRegistrationHelper registrationHelper, IBootstrapperContainerExtensionOptions options): base(registrationHelper)
        {
            Options= new WindsorOptions(options);
            facilities = new List<IFacility>();
            Bootstrapper.Excluding.Assembly("Castle");
            Bootstrapper.Excluding.Assembly("Castle.Facilities.FactorySupport");
        }

        public void AddFacility(IFacility facility)
        {
            facilities.Add(facility);
        }

        public void InitializeContainer(IWindsorContainer aContainer)
        {
            Container = container = aContainer.AddFacility<FactorySupportFacility>();
            facilities.ForEach(f => container.AddFacility(f));
        }

        protected override void InitializeContainer()
        {
            InitializeContainer(Options.Container ?? new WindsorContainer());
        }

        protected override void RegisterImplementationsOfIRegistration()
        {
            CheckContainer();
            if(Options.AutoRegistration) AutoRegister();
            RegisterAll<IBootstrapperRegistration>();
            RegisterAll<IWindsorRegistration>();
            RegisterAll<IWindsorInstaller>();
        }

        protected override void InvokeRegisterForImplementationsOfIRegistration()
        {
            CheckContainer();
            container.ResolveAll<IBootstrapperRegistration>().ForEach(r => r.Register(this));
            container.ResolveAll<IWindsorRegistration>().ForEach(r => r.Register(container));
            container.Install(container.ResolveAll<IWindsorInstaller>());
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
                container.Register(Component.For(typeof(TTarget)).ImplementedBy<TImplementation>());

        }

        public override void Register<TTarget>(TTarget implementation)
        {
            CheckContainer();
            if (!container.Kernel.HasComponent(typeof(TTarget).Name) ||
                container.Resolve<TTarget>().GetType() != implementation.GetType())
                container.Register(Component.For(typeof(TTarget)).Instance(implementation));
        }

        public override void RegisterAll<TTarget>()
        {
            CheckContainer();
            Registrator.GetAssemblies().ToList().ForEach(
                a => container.Register(Classes.FromAssembly(a).BasedOn<TTarget>().WithService.Base()));
        }

        protected override void ResetContainer()
        {
            container = null;
            Container = null;
        }
    }
}
