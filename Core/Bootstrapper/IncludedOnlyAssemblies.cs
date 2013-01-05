using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bootstrap.Extensions;

namespace Bootstrap
{
    public class IncludedOnlyAssemblies: IIncludedOnlyAssemblies
    {
        public List<Assembly> Assemblies { get; set; }

        public IncludedOnlyAssemblies()
        {
            Assemblies = new List<Assembly>();
        }

        public IIncludedOnlyAssemblies Assembly(Assembly assembly)
        {
            if(Assemblies.Count == 0) Assemblies.AddRange(BootstrapperAssemblies());
            if(!Assemblies.Contains(assembly)) Assemblies.Add(assembly);
            return this;
        }

        public IIncludedOnlyAssemblies AssemblyRange(IEnumerable<Assembly> assemblies)
        {
            Assemblies.AddRange(assemblies);
            return this;
        }

        public IIncludedOnlyAssemblies AndAssembly(Assembly assembly)
        {
            return Assembly(assembly);
        }

        public IIncludedOnlyAssemblies AndAssemblyRange(IEnumerable<Assembly> assemblies)
        {
            return AssemblyRange(assemblies);
        }

        public IIncludedAssemblies Including
        {
            get { return Bootstrapper.Including; }
        }

        public IExcludedAssemblies Excluding
        {
            get { return Bootstrapper.Excluding; }
        }

        public BootstrapperExtensions With
        {
            get { return Bootstrapper.With; }
        }

        public void Start()
        {
            Bootstrapper.Start();
        }

        private static IEnumerable<Assembly> BootstrapperAssemblies()
        {
            var bootstrapperAssemblies = new List<Assembly>();
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            loadedAssemblies.ForEach(a =>
                                         {
                                             if (a.FullName.StartsWith("Bootstrapper"))
                                                 bootstrapperAssemblies.Add(a);
                                         });
            return bootstrapperAssemblies;
        }
    }
}
