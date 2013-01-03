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
        protected IRegistrationHelper RegistrationHelper;
        public abstract void SetServiceLocator();
        public abstract void ResetServiceLocator();
        public abstract T Resolve<T>() where T : class;
        public abstract IList<T> ResolveAll<T>();
        public abstract void Register<TTarget, TImplementation>() where TTarget : class where TImplementation : class, TTarget;
        public abstract void Register<TTarget>(TTarget implementation) where TTarget : class;
        public abstract void RegisterAll<TTarget>() where TTarget : class;

        public object Container { get; protected set; }

        protected BootstrapperContainerExtension(IRegistrationHelper registrationHelper)
        {
            RegistrationHelper = registrationHelper;
            Bootstrapper.Excluding.Assembly("Microsoft.Practices");
        }

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
            RegistrationHelper.GetAssemblies()
                .ForEach(a => a.GetExportedTypes().Where(t => !t.IsGenericType && !t.IsAbstract)
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

        protected void CheckContainer()
        {
            if (Container == null) throw new NoContainerException();
        }
    }
}