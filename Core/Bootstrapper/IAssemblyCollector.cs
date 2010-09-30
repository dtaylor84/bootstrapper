using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bootstrap
{
    public interface IAssemblyCollector
    {
        IList<String> AssemblyNames { get; }
        IList<Assembly> Assemblies { get; }
        IBootstrapperContainerExtension InAssemblyNamed(string assemblyName);
        IBootstrapperContainerExtension InAssembly(Assembly assembly);
    }
}
