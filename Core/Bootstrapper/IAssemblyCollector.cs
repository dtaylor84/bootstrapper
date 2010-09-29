using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bootstrapper
{
    public interface IAssemblyCollector
    {
        IList<String> AssemblyNames { get; }
        IList<Assembly> Assemblies { get; }
        IBootstrapperContainerExtension InAssemblyNamed(string assemblyName);
        IBootstrapperContainerExtension InAssembly(Assembly assembly);
    }
}
