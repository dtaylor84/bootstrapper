using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bootstrap.Extensions.Containers
{
    public abstract class BootstrapperContainerExtension : IBootstrapperContainerExtension
    {
        protected internal IRegistrationHelper Registrator;
        public abstract void SetServiceLocator();
        public abstract void ResetServiceLocator();
        public abstract T Resolve<T>() where T : class;
        public abstract IList<T> ResolveAll<T>();
        public abstract void RegisterAll(Type target);
        public abstract void Register<TTarget, TImplementation>() where TTarget : class where TImplementation : class, TTarget;
        public abstract void Register<TTarget>(TTarget implementation) where TTarget : class;
        public abstract void RegisterAll<TTarget>() where TTarget : class;
        public object Container { get; protected set; }

        protected BootstrapperContainerExtension(IRegistrationHelper registrationHelper)
        {
            Registrator = registrationHelper;
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
            Registrator
                .GetAssemblies()
                .SelectMany(a => a.GetExportedTypes())
                .Where(t => !t.IsGenericType && !t.IsAbstract)
                .Select(t => new { Type = t, DefaultInterface = t.GetInterface("I"+ t.Name) })
                .Where(t => t.DefaultInterface!=null)
                .ForEach(t =>Register(t.DefaultInterface, t.Type));
        }

        protected void CheckContainer()
        {
            if (Container == null) throw new NoContainerException();
        }

        protected abstract void InitializeContainer();
        protected abstract void RegisterImplementationsOfIRegistration();
        protected abstract void InvokeRegisterForImplementationsOfIRegistration();
        protected abstract void ResetContainer();

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