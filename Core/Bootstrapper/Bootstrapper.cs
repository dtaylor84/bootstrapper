using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bootstrapper
{
    public static class Bootstrapper
    {
        private static readonly BootstrapperExtensions Extensions;
        internal static object Container { get; set; }
        internal static Assembly StartCallingAssembly { get; set; }

        public static BootstrapperExtensions With { get { return Extensions; } }

        static Bootstrapper()
        {
            Extensions = new BootstrapperExtensions();
        }

        public static void Start()
        {
            StartCallingAssembly = StartCallingAssembly ?? Assembly.GetCallingAssembly();

            foreach (var extension in Extensions.GetExtensions())
                extension.Run();
        }


        public static void Reset()
        {
            foreach (var extension in Extensions.GetExtensions())
                extension.Reset();
        }

        public static void ClearExtensions()
        {
            Reset();
            Extensions.ClearExtensions();
        }

        public static IList<IBootstrapperExtension> GetExtensions()
        {
            return With.GetExtensions();
        }

        public static IBootstrapperContainerExtension GetContainerExtension()
        {
            return With.GetContainerExtension();
        }

        public static Assembly GetStartCallingAssembly()
        {
            return StartCallingAssembly;
        }

        public static Object GetContainer()
        {
            return Container;
        }
    }
}
