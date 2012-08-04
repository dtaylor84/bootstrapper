using System.Collections.Generic;
using System.Reflection;
using Bootstrap.Extensions;

namespace Bootstrap
{
    public interface IIncludedAssemblies
    {
        List<Assembly> Assemblies { get; set; }
        IIncludedAssemblies Assembly(Assembly assembly);
        IIncludedAssemblies AssemblyRange(IEnumerable<Assembly> assemblies);
        IIncludedAssemblies AndAssembly(Assembly assembly);
        IIncludedAssemblies AndAssemblyRange(IEnumerable<Assembly> assemblies);
        IIncludedOnlyAssemblies IncludingOnly { get; }
        IExcludedAssemblies Excluding { get; }
        BootstrapperExtensions With { get; }
        void Start();
    }
}
