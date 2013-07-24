using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions.Containers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace Bootstrap.Unity
{
    public class UnityExtension: BootstrapperContainerExtension
    {
        private IUnityContainer container;
        public UnityOptions Options { get; private set; }

        public UnityExtension(IRegistrationHelper registrationHelper, IBootstrapperContainerExtensionOptions options): base(registrationHelper)
        {
            Options = new UnityOptions(options);
            Bootstrapper.Excluding.Assembly("Microsoft.Practices");
        }

        public void InitializeContainer(IUnityContainer aContainer)
        {
            Container = container = aContainer;
        }

        protected override void InitializeContainer()
        {
            InitializeContainer(Options.Container ?? new UnityContainer());
        }

        protected override void RegisterImplementationsOfIRegistration()
        {
            CheckContainer();
            if (Options.AutoRegistration) AutoRegister();
            RegisterAll<IBootstrapperRegistration>();
            RegisterAll<IUnityRegistration>();
        }

        protected override void InvokeRegisterForImplementationsOfIRegistration()
        {
            CheckContainer();
            container.ResolveAll<IBootstrapperRegistration>().ToList().ForEach(r => r.Register(this));
            container.ResolveAll<IUnityRegistration>().ToList().ForEach(r => r.Register(container));
        }

        public override void SetServiceLocator()
        {
            CheckContainer();
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
        }

        public override void ResetServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => null);
        }
        
        public override T Resolve<T>()
        {
            CheckContainer();
            return container.IsRegistered<T>() 
                ? container.Resolve<T>() 
                : (container.ResolveAll<T>().FirstOrDefault());
        }

        public override IList<T> ResolveAll<T>()
        {
            CheckContainer();
            return container.ResolveAll<T>().ToList();
        }

        public override void Register<TTarget, TImplementation>()
        {
            CheckContainer();
            container.RegisterType<TTarget, TImplementation>(typeof(TImplementation).Name);
        }

        public override void Register<TTarget>(TTarget implementation)
        {
            CheckContainer();
            container.RegisterInstance(implementation.GetType().Name, implementation);
        }

        public override void RegisterAll<TTarget>()
        {
            CheckContainer();
            Registrator.GetAssemblies().ToList().ForEach(
                a => Registrator.GetTypesImplementing<TTarget>(a).ToList().ForEach(
                    t => container.RegisterType(typeof (TTarget), t, t.FullName)));
        }

        protected override void ResetContainer()
        {
            container = null;
            Container = null;
        }
    }
}
