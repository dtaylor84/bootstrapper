using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bootstrap
{
    public class IncludedOnlyAssemblies: BootstrapperOption, IIncludedOnlyAssemblies
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

        private static IEnumerable<Assembly> BootstrapperAssemblies()
        {
            return AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(a => a.FullName.StartsWith("Bootstrapper"));
        }
    }
}
