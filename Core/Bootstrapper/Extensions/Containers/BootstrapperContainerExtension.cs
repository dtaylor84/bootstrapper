using System.Collections.Generic;

namespace Bootstrap.Extensions.Containers
{
    public abstract class BootstrapperContainerExtension : IBootstrapperContainerExtension
    {
        protected abstract void InitializeContainer();
        protected abstract void RegisterImplementationsOfIRegistration();
        protected abstract void InvokeRegisterForImplementationsOfIRegistration();
        protected abstract void ResetContainer();
        public abstract void SetServiceLocator();
        public abstract void ResetServiceLocator();
        public abstract T Resolve<T>();
        public abstract IList<T> ResolveAll<T>();
        public abstract void Register<TTarget, TImplementation>() where TImplementation : TTarget;
        public abstract void Register<TTarget>(TTarget implementation);
        public abstract void RegisterAll<TTarget>();

        public void  Run()
        {
            InitializeContainer();
            InitializeRegistrations();
        }

        private void InitializeRegistrations()
        {
            RegisterImplementationsOfIRegistration();
            InvokeRegisterForImplementationsOfIRegistration();
        }

        public void Reset()
        {
            ResetContainer();
        }

        public object Container { get; protected set; }
    }
}