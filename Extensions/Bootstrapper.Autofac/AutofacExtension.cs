using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Core;
using Bootstrap.Extensions.Containers;
using Autofac;
using CommonServiceLocator.AutofacAdapter.Unofficial;
using Microsoft.Practices.ServiceLocation;

namespace Bootstrap.Autofac
{
    public class AutofacExtension : BootstrapperContainerExtension
    {
        private IContainer container;
        public AutofacOptions Options { get; private set; }

        public AutofacExtension(IRegistrationHelper registrationHelper, IBootstrapperContainerExtensionOptions options): base(registrationHelper)
        {
            Options = new AutofacOptions(options);
            Bootstrapper.Excluding.Assembly("Autofac");
        }

        public void InitializeContainer(IContainer aContainer)
        {
            Container = container = aContainer;
        }

        protected override void InitializeContainer()
        {
            InitializeContainer(Options.Container ?? new ContainerBuilder().Build());
        }

        protected override void RegisterImplementationsOfIRegistration()
        {
            CheckContainer();
            if (Options.AutoRegistration) AutoRegister();
            RegisterAll<IBootstrapperRegistration>();
            RegisterAll<IAutofacRegistration>();
            RegisterAll<IModule>();
        }

        protected override void InvokeRegisterForImplementationsOfIRegistration()
        {
            CheckContainer();

            container.Resolve<IEnumerable<IBootstrapperRegistration>>().ToList().ForEach(r => r.Register(this));
            container.Resolve<IEnumerable<IAutofacRegistration>>().ToList().ForEach(r => UpdateContainer(r.Register));
            container.Resolve<IEnumerable<IModule>>().ToList().ForEach(UpdateContainer);
        }

        protected override void ResetContainer()
        {
            container = null;
            Container = null;
        }

        public override void RegisterAll<TTarget>()
        {
            CheckContainer();

            UpdateContainer(cb => Registrator.GetAssemblies().ToList()
                                    .ForEach(a => Registrator.GetTypesImplementing<TTarget>(a).ToList()
                                        .ForEach(t => cb.RegisterType(t).As<TTarget>())));
        }

        public override void SetServiceLocator()
        {
            CheckContainer();
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
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
            return container.Resolve<IEnumerable<T>>().ToList();
        }

        public override void Register<TTarget, TImplementation>()
        {
            CheckContainer();
            UpdateContainer(cb => cb.RegisterType<TImplementation>().As<TTarget>());
        }

        public override void Register<TTarget>(TTarget implementation)
        {
            CheckContainer();
            UpdateContainer(cb => cb.RegisterInstance((object)implementation).As<TTarget>());
        }

        private void UpdateContainer(Action<ContainerBuilder> registrationBuilder)
        {
            var containerBuilder = new ContainerBuilder();
            registrationBuilder(containerBuilder);
            containerBuilder.Update(container);
        }

        private void UpdateContainer(IModule module)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(module);
            containerBuilder.Update(container);
            
        }
    }
}
