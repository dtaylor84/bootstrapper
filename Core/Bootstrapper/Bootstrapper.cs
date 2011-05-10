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

        static Bootstrapper()
        {
            Extensions = new BootstrapperExtensions();
        }

        public static void Start()
        {
            foreach (var extension in Extensions.GetExtensions())
                extension.Run();
        }

        public static void Reset()
        {
            foreach (var extension in Extensions.GetExtensions().Reverse())
                extension.Reset();
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
    }
}
