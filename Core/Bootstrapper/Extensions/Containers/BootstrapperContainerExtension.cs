using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        public object Container { get; protected set; }

        public void  Run()
        {
            InitializeContainer();
            InitializeRegistrations();
        }

        public void Reset()
        {
            ResetContainer();
        }

        protected void AutoRegister()
        {
            RegistrationHelper.GetAssemblies().ToList()
                .ForEach(a => a.GetExportedTypes().Where(t => !t.IsGenericType && !t.IsAbstract).ToList()
                    .ForEach(t =>
                                 {
                                     var defaultInterfaceName = string.Format("I{0}", t.Name);
                                     var defaultInterface = t.GetInterface(defaultInterfaceName);
                                     if (defaultInterface != null)
                                         Register(defaultInterface,t);
                                 }));
        }

        private void InitializeRegistrations()
        {
            RegisterImplementationsOfIRegistration();
            InvokeRegisterForImplementationsOfIRegistration();
        }

        private void Register(Type target, Type implementation)
        {
            var genericClass = typeof(RegistrationInvoker<,>);
            var invokerClass = genericClass.MakeGenericType(target,implementation);
            var invoker = Activator.CreateInstance(invokerClass,this);
            invoker.GetType().InvokeMember("Register", BindingFlags.Default | BindingFlags.InvokeMethod, null, invoker, null);
        }
    }
}