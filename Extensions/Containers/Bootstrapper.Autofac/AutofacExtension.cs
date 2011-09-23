using System;
using System.Collections.Generic;
using System.Linq;
using AutofacContrib.CommonServiceLocator;
using Bootstrap.Extensions.Containers;
using global::Autofac;
using Microsoft.Practices.ServiceLocation;

namespace Bootstrap.Autofac
{
    public class AutofacExtension : BootstrapperContainerExtension
    {
        private IContainer container;
        public IBootstrapperContainerExtensionOptions Options { get; private set; }

        public AutofacExtension()
        {
            Options = new BootstrapperContainerExtensionOptions();
            Bootstrapper.Excluding.Assembly("Autofac");
        }

        public void InitializeContainer(IContainer aContainer)
        {
            container = aContainer;
            Container = container;
        }

        protected override void InitializeContainer()
        {
            container = new ContainerBuilder().Build();
            Container = container;
        }

        protected override void RegisterImplementationsOfIRegistration()
        {
            CheckContainer();
            if (Options.AutoRegistration) AutoRegister();
            RegisterAll<IBootstrapperRegistration>();
            RegisterAll<IAutofacRegistration>();
        }

        protected override void InvokeRegisterForImplementationsOfIRegistration()
        {
            CheckContainer();

            container.Resolve<IEnumerable<IBootstrapperRegistration>>().ToList().ForEach(r => r.Register(this));
            container.Resolve<IEnumerable<IAutofacRegistration>>().ToList().ForEach(r => UpdateContainer(cb => r.Register(cb)));
        }

        protected override void ResetContainer()
        {
            container = null;
            Container = null;
        }

        public override void RegisterAll<TTarget>()
        {
            CheckContainer();

            UpdateContainer(cb =>
            {
                RegistrationHelper.GetAssemblies().ToList().ForEach(a =>
                {
                    RegistrationHelper.GetTypesImplementing<TTarget>(a).ToList().ForEach(t => cb.RegisterType(t).As<TTarget>());
                });
            });
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
    }
}
