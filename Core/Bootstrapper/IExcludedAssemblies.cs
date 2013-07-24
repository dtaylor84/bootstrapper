using System.Collections.Generic;

namespace Bootstrap
{
    public interface IExcludedAssemblies : IBootstrapperOption
    {
        List<string> Assemblies { get; set; }
        IExcludedAssemblies Assembly(string assembly);
        IExcludedAssemblies AndAssembly(string assembly);
    }
}
