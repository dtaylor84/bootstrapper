using System.Collections.Generic;
using System.Reflection;

namespace Bootstrap
{
    public interface IIncludedOnlyAssemblies: IBootstrapperOption
    {
        List<Assembly> Assemblies { get; set; }
        IIncludedOnlyAssemblies Assembly(Assembly assembly);
        IIncludedOnlyAssemblies AssemblyRange(IEnumerable<Assembly> assemblies);
        IIncludedOnlyAssemblies AndAssembly(Assembly assembly);
        IIncludedOnlyAssemblies AndAssemblyRange(IEnumerable<Assembly> assemblies);
    }
}
