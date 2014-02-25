using System.Collections.Generic;
using System.Reflection;

namespace Bootstrap
{
    public class IncludedAssemblies: BootstrapperOption, IIncludedAssemblies
    {
        public List<Assembly> Assemblies { get; set; }

        public IncludedAssemblies()
        {
            Assemblies = new List<Assembly>();
        }

        public IIncludedAssemblies Assembly(Assembly assembly)
        {
            Assemblies.Add(assembly);
            return this;
        }

        public IIncludedAssemblies AssemblyRange(IEnumerable<Assembly> assemblies)
        {
            Assemblies.AddRange(assemblies);
            return this;
        }

        public IIncludedAssemblies AndAssembly(Assembly assembly)
        {
            return Assembly(assembly);
        }

        public IIncludedAssemblies AndAssemblyRange(IEnumerable<Assembly> assemblies)
        {
            return AssemblyRange(assemblies);
        }
    }
}
