using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap
{
    public static class Bootstrapper
    {
        internal static IRegistrationHelper Registrator;

        public static object Container {get { return ContainerExtension == null ? null : ContainerExtension.Container; }}
        public static IBootstrapperContainerExtension ContainerExtension { get; set; }
        public static IRegistrationHelper RegistrationHelper
        {
            get
            {
                if (Registrator == null) LookForTypesIn.LoadedAssemblies();
                return Registrator;
            }
            set { Registrator = value; }
        }

        public static BootstrapperExtensions With { get; private set; }
        public static IExcludedAssemblies Excluding { get; private set; }
        public static IIncludedAssemblies Including { get; private set; }
        public static IIncludedOnlyAssemblies IncludingOnly { get; private set; }
        public static IAssemblySetOptions LookForTypesIn { get; private set; }

        static Bootstrapper()
        {
            InitializeExcludedAndIncludedAssemblies();
        }

        public static void ClearExtensions()
        {
            Reset();
            ContainerExtension = null;
            Registrator = null;
            With.ClearExtensions();
        }

        public static IList<IBootstrapperExtension> GetExtensions()
        {
            return With.GetExtensions();
        }

        public static void Start()
        {
            foreach (var extension in With.GetExtensions())
                extension.Run();
        }

        public static void Reset()
        {
            foreach (var extension in With.GetExtensions().Reverse())
                extension.Reset();
            InitializeExcludedAndIncludedAssemblies();
        }

        private static void InitializeExcludedAndIncludedAssemblies()
        {
            With = new BootstrapperExtensions();
            Excluding = new ExcludedAssemblies();
            Including = new IncludedAssemblies();
            IncludingOnly = new IncludedOnlyAssemblies();
            LookForTypesIn = new AssemblySetOptions();
        }
    }
}
