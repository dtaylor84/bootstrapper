using System.Collections.Generic;
using System.Reflection;

namespace Bootstrap
{
    public interface IIncludedAssemblies: IBootstrapperOption
    {
        List<Assembly> Assemblies { get; set; }
        IIncludedAssemblies Assembly(Assembly assembly);
        IIncludedAssemblies AssemblyRange(IEnumerable<Assembly> assemblies);
        IIncludedAssemblies AndAssembly(Assembly assembly);
        IIncludedAssemblies AndAssemblyRange(IEnumerable<Assembly> assemblies);
    }
}
