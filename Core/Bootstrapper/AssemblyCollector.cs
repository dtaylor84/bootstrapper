using System.Collections.Generic;
using System.Reflection;

namespace Bootstrap
{
    public class AssemblyCollector: IAssemblyCollector
    {
        private readonly IBootstrapperContainerExtension extension;
        private readonly List<string> assemblyNames = new List<string>();
        private readonly List<Assembly> assemblies =  new List<Assembly>();

        public AssemblyCollector(IBootstrapperContainerExtension extension)
        {
            this.extension = extension;
        }

        public IList<string> AssemblyNames { get { return assemblyNames.AsReadOnly(); } }

        public IList<Assembly> Assemblies { get { return assemblies.AsReadOnly(); } }

        public IBootstrapperContainerExtension InAssemblyNamed(string assemblyName)
        {
            if(!assemblyNames.Contains(assemblyName)) assemblyNames.Add(assemblyName);
            return extension;
        }

        public IBootstrapperContainerExtension InAssembly(Assembly assembly)
        {
            if(assembly!=null && ! assemblies.Contains(assembly)) assemblies.Add(assembly);
            return extension;
        }
    }
}
