using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap
{
    public static class Bootstrapper
    {
        private static readonly BootstrapperExtensions Extensions;

        public static object Container {get { return ContainerExtension == null ? null : ContainerExtension.Container; }}
        public static BootstrapperExtensions With { get { return Extensions; } }
        public static IBootstrapperContainerExtension ContainerExtension { get; set; }
        public static IExcludedAssemblies Excluding { get; private set; }
        public static IIncludedAssemblies Including { get; private set; }

        static Bootstrapper()
        {
            Extensions = new BootstrapperExtensions();
            InitializeExcludedAndIncludedAssemblies();
        }

        public static void ClearExtensions()
        {
            Reset();
            ContainerExtension = null;
            Extensions.ClearExtensions();
        }

        public static IList<IBootstrapperExtension> GetExtensions()
        {
            return With.GetExtensions();
        }

        public static void Start()
        {
            foreach (var extension in Extensions.GetExtensions())
                extension.Run();
        }

        public static void Reset()
        {
            InitializeExcludedAndIncludedAssemblies();
            foreach (var extension in Extensions.GetExtensions().Reverse())
                extension.Reset();
        }

        private static void InitializeExcludedAndIncludedAssemblies()
        {
            Excluding = new ExcludedAssemblies();
            Including = new IncludedAssemblies();
        }
    }
}
