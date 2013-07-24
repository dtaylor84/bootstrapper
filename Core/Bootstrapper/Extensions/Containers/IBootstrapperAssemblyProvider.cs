using System.Collections.Generic;
using System.Reflection;

namespace Bootstrap.Extensions.Containers
{
    public interface IBootstrapperAssemblyProvider
    {
        IEnumerable<Assembly> GetAssemblies();
    }
}