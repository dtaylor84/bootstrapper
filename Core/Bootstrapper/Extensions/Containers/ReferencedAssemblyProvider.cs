using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;

namespace Bootstrap.Extensions.Containers
{
    public class ReferencedAssemblyProvider: IBootstrapperAssemblyProvider 
    {
        public IEnumerable<Assembly> GetAssemblies()
        {
            return BuildManager.GetReferencedAssemblies().Cast<Assembly>();
        }
    }
}