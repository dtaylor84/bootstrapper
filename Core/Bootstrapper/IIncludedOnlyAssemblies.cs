using System.Collections.Generic;
using System.Reflection;
using Bootstrap.Extensions;

namespace Bootstrap
{
    public interface IIncludedOnlyAssemblies
    {
        List<Assembly> Assemblies { get; set; }
        IIncludedOnlyAssemblies Assembly(Assembly assembly);
        IIncludedOnlyAssemblies AndAssembly(Assembly assembly);
        IIncludedAssemblies Including { get; }
        IExcludedAssemblies Excluding { get; }
        BootstrapperExtensions With { get; }
        void Start();
    }
}
