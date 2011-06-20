using System.Collections.Generic;
using System.Reflection;
using Bootstrap.Extensions;

namespace Bootstrap
{
    public interface IIncludedAssemblies
    {
        List<Assembly> Assemblies { get; set; }
        IIncludedAssemblies Assembly(Assembly assembly);
        IIncludedAssemblies AndAssembly(Assembly assembly);
        IExcludedAssemblies Excluding { get; }
        BootstrapperExtensions With { get; }
        void Start();
    }
}
