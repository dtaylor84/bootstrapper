using System.Collections.Generic;
using Bootstrap.Extensions;

namespace Bootstrap
{
    public interface IExcludedAssemblies
    {
        List<string> Assemblies { get; set; }
        IExcludedAssemblies Assembly(string assembly);
        IExcludedAssemblies AndAssembly(string assembly);
        BootstrapperExtensions With { get; }
        IIncludedAssemblies Including { get; }
        void Start();
    }
}
