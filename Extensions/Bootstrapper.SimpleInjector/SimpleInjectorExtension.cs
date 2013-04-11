using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions.Containers;
using CommonServiceLocator.SimpleInjectorAdapter;
using Microsoft.Practices.ServiceLocation;
using SimpleInjector;

namespace Bootstrap.SimpleInjector
{
    public class SimpleInjectorExtension: BootstrapperContainerExtension
    {
        private Container container;
        public SimpleInjectorOptions Options { get; private set; }

        public SimpleInjectorExtension(IRegistrationHelper registrationHelper, IBootstrapperContainerExtensionOptions options): base(registrationHelper)
        {
            Options = new SimpleInjectorOptions(options);
            Bootstrapper.Excluding.Assembly("SimpleInjector");
        }

        public void InitializeContainer(Container aContainer)
        {
            Container = container = aContainer;
        }

        protected override void InitializeContainer()
        {
            InitializeContainer(Options.Container ?? new Container());
        }

        protected override void RegisterImplementationsOfIRegistration()
        {
            CheckContainer();
            if (Options.AutoRegistration) AutoRegister();
            RegisterAll<IBootstrapperRegistration>();
            RegisterAll<ISimpleInjectorRegistration>();
        }

        protected override void InvokeRegisterForImplementationsOfIRegistration()
        {
            CheckContainer();
            RegistrationHelper.GetInstancesOfTypesImplementing<IBootstrapperRegistration>().ForEach(r => r.Register(this));
            RegistrationHelper.GetInstancesOfTypesImplementing<ISimpleInjectorRegistration>().ForEach(r => r.Register(container));
        }

        protected override void ResetContainer()
        {
            Container = container = null;
        }

        public override void RegisterAll<TTarget>()  
        {
            CheckContainer();
            container.RegisterAll<TTarget>(
                RegistrationHelper
                .GetAssemblies()
                .SelectMany(a => RegistrationHelper.GetTypesImplementing<TTarget>(a))
                );
        }

        public override void SetServiceLocator()
        {
            CheckContainer();
            ServiceLocator.SetLocatorProvider(() => new SimpleInjectorServiceLocatorAdapter(container));
        }

        public override void ResetServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => null);
        }

        public override T Resolve<T>()
        {
            CheckContainer();
            return container.GetInstance<T>();
        }

        public override IList<T> ResolveAll<T>()
        {
            CheckContainer();
            return container.GetAllInstances<T>().ToList();
        }

        public override void Register<TTarget, TImplementation>()
        {
            CheckContainer();
            container.Register<TTarget,TImplementation>();
        }

        public override void Register<TTarget>(TTarget implementation)
        {
            CheckContainer();
            container.RegisterSingle(implementation);
        }
    }
}
