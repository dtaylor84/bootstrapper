using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions.Containers;
using CommonServiceLocator.NinjectAdapter.Unofficial;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using Ninject.Modules;

namespace Bootstrap.Ninject
{
    public class NinjectExtension: BootstrapperContainerExtension
    {
        private IKernel container;        
        public NinjectOptions Options { get; private set; }

        public NinjectExtension(IRegistrationHelper registrationHelper, IBootstrapperContainerExtensionOptions options): base(registrationHelper)
        {
            Options = new NinjectOptions(options);
            Bootstrapper.Excluding.Assembly("Ninject");
        }

        public void InitializeContainer(IKernel aContainer)
        {
            Container = container = aContainer;
        }

        protected override void InitializeContainer()
        {
            InitializeContainer(Options.Container ?? new StandardKernel());
        }

        protected override void RegisterImplementationsOfIRegistration()
        {
            CheckContainer();
            if (Options.AutoRegistration) AutoRegister();
            RegisterAll<IBootstrapperRegistration>();
            RegisterAll<INinjectRegistration>();
            RegisterAll<INinjectModule>();
        }

        protected override void InvokeRegisterForImplementationsOfIRegistration()
        {
            CheckContainer();
            container.GetAll<IBootstrapperRegistration>().ToList().ForEach(r => r.Register(this));
            container.GetAll<INinjectRegistration>().ToList().ForEach(r => r.Register(container));
            container.Load(container.GetAll<INinjectModule>());
        }

        protected override void ResetContainer()
        {
            container = null;
            Container = null;
        }

        public override void RegisterAll<TTarget>()
        {
            CheckContainer();
            RegistrationHelper.GetAssemblies().ToList().ForEach(
                a => RegistrationHelper.GetTypesImplementing<TTarget>(a).ToList().ForEach(
                    t => container.Bind<TTarget>().To(t)));
        }

        public override void SetServiceLocator()
        {
            CheckContainer();
            ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(container));
        }

        public override void ResetServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => null);
        }

        public override T Resolve<T>()
        {
            CheckContainer();
            return container.Get<T>();
        }

        public override IList<T> ResolveAll<T>()
        {
            CheckContainer();
            return container.GetAll<T>().ToList();
        }

        public override void Register<TTarget, TImplementation>()
        {
            CheckContainer();
            container.Bind<TTarget>().To<TImplementation>();
        }

        public override void Register<TTarget>(TTarget implementation)
        {
            CheckContainer();
            container.Bind<TTarget>().ToConstant(implementation);
        }
    }
}
