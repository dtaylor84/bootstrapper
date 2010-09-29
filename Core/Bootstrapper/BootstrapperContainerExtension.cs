using System.Reflection;
using Microsoft.Practices.ServiceLocation;

namespace Bootstrapper
{
    public abstract class BootstrapperContainerExtension : BootstrapperExtension, IBootstrapperContainerExtension
    {
        private readonly AssemblyCollector registrationAssemblyCollector;
        private readonly AssemblyCollector mapAssemblyCollector;
        private readonly AssemblyCollector startupTaskAssemblyCollector;

        public IAssemblyCollector LookForRegistrations { get { return registrationAssemblyCollector; } }
        public IAssemblyCollector LookForMaps { get { return mapAssemblyCollector; } }
        public IAssemblyCollector LookForStartupTasks { get { return startupTaskAssemblyCollector; } }

        protected abstract void InitializeContainer();
        protected abstract void RegisterImplementationsOfIRegistration();
        protected abstract void InvokeRegisterForImplementationsOfIRegistration();
        protected abstract void InitializeServiceLocator();
        protected abstract void ResetContainer();

        protected BootstrapperContainerExtension()
        {
            registrationAssemblyCollector = new AssemblyCollector(this);
            mapAssemblyCollector = new AssemblyCollector(this);
            startupTaskAssemblyCollector = new AssemblyCollector(this);
        }

        public override void  Run()
        {
            AddDefaultAssembliesToRegistrationsMapsAndStartupTasks();
            InitializeContainer();
            InitializeRegistrations();
            InitializeServiceLocator();
            RunStartupTasks();
        }

        private void AddDefaultAssembliesToRegistrationsMapsAndStartupTasks()
        {
            AddDefaultAssembliesToAssemblyCollector(LookForRegistrations);
            AddDefaultAssembliesToAssemblyCollector(LookForMaps);
            AddDefaultAssembliesToAssemblyCollector(LookForStartupTasks);
        }

        private static void AddDefaultAssembliesToAssemblyCollector(IAssemblyCollector look)
        {
            look.InAssembly(Assembly.GetAssembly(typeof(Bootstrapper)));
            look.InAssembly(Assembly.GetAssembly(Bootstrapper.GetContainerExtension().GetType()));
            look.InAssembly(Bootstrapper.GetStartCallingAssembly());
            look.InAssembly(Assembly.GetEntryAssembly());
            foreach (var extension in Bootstrapper.GetExtensions())
                look.InAssembly(Assembly.GetAssembly(extension.GetType()));
        }

        private void InitializeRegistrations()
        {
            RegisterImplementationsOfIRegistration();
            InvokeRegisterForImplementationsOfIRegistration();
        }

        private static void RunStartupTasks()
        {
            foreach(var task in ServiceLocator.Current.GetAllInstances<IStartupTask>())
                task.Run();
        }

        public override void Reset()
        {
            ResetStartupTasks();
            ResetServiceLocator();
            ResetContainer();
        }

        private static void ResetStartupTasks()
        {
            if (ServiceLocator.Current == null) return;
            foreach (var task in ServiceLocator.Current.GetAllInstances<IStartupTask>())
                task.Reset();
        }

        private static void ResetServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => null);
        }
    }
}