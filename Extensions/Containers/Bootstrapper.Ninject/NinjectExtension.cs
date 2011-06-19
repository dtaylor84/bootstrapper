using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions.Containers;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using NinjectAdapter;

namespace Bootstrap.Ninject
{
    public class NinjectExtension: BootstrapperContainerExtension
    {
        private IKernel container;
        public IBootstrapperContainerExtensionOptions Options { get; private set; }

        public NinjectExtension()
        {
            Options = new BootstrapperContainerExtensionOptions();
            Bootstrapper.Excluding.Assembly("Ninject");
        }

        public void InitializeContainer(IKernel aContainer)
        {
            container = aContainer;
            Container = container;
        }

        protected override void InitializeContainer()
        {
            container = new StandardKernel();
            Container = container;
        }

        protected override void RegisterImplementationsOfIRegistration()
        {
            CheckContainer();
            if (Options.AutoRegistration) AutoRegister();
            RegisterAll<IBootstrapperRegistration>();
            RegisterAll<INinjectRegistration>();
        }

        protected override void InvokeRegisterForImplementationsOfIRegistration()
        {
            CheckContainer();
            container.GetAll<IBootstrapperRegistration>().ToList().ForEach(r => r.Register(this));
            container.GetAll<INinjectRegistration>().ToList().ForEach(r => r.Register(container));
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
