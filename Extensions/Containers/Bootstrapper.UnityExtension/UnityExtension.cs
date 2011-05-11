using System;
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

        public void InitializeContainer(IUnityContainer aContainer)
        {
            container = aContainer;
            Container = container;
        }

        protected override void InitializeContainer()
        {
            container = new UnityContainer();
            Container = container;
        }

        protected override void RegisterImplementationsOfIRegistration()
        {
            RegisterAll<IBootstrapperRegistration>();
            RegisterAll<IUnityRegistration>();
        }

        protected override void InvokeRegisterForImplementationsOfIRegistration()
        {
            container.ResolveAll<IBootstrapperRegistration>().ToList().ForEach(r => r.Register(this));
            container.ResolveAll<IUnityRegistration>().ToList().ForEach(r => r.Register(container));
        }

        public override void SetServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
        }

        public override void ResetServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => null);
        }
        
        public override T Resolve<T>()
        {
            return container.IsRegistered<T>() 
                ? container.Resolve<T>() 
                : (container.ResolveAll<T>().FirstOrDefault());
        }

        public override IList<T> ResolveAll<T>()
        {
            return container.ResolveAll<T>().ToList();
        }

        public override void Register<TTarget, TImplementation>()
        {
            container.RegisterType<TTarget, TImplementation>(typeof(TImplementation).Name);
        }

        public override void Register<TTarget>(TTarget implementation)
        {
            container.RegisterInstance(implementation.GetType().Name, implementation);
        }

        public override void RegisterAll<TTarget>()
        {
            AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToList().ForEach(
                a => RegistrationHelper.GetTypesImplementing<TTarget>(a).ToList().ForEach(
                    t => container.RegisterType(typeof (TTarget), t, t.Name)));
        }

        protected override void ResetContainer()
        {
            container = null;
            Container = null;
        }
    }
}
