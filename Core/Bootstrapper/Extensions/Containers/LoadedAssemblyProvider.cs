using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bootstrap.Extensions.Containers
{
    public class LoadedAssemblyProvider:IBootstrapperAssemblyProvider
    {
        public IEnumerable<Assembly> GetAssemblies() {return AppDomain.CurrentDomain.GetAssemblies();}
    }
}