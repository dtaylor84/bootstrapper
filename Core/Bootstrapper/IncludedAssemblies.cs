using System.Collections.Generic;
using System.Reflection;
using Bootstrap.Extensions;

namespace Bootstrap
{
    public class IncludedAssemblies: IIncludedAssemblies
    {
        public List<Assembly> Assemblies { get; set; }

        public IncludedAssemblies()
        {
            Assemblies = new List<Assembly>();
        }

        public IIncludedAssemblies Assembly(Assembly assembly)
        {
            Assemblies.Add(assembly);
            return this;
        }

        public IIncludedAssemblies AndAssembly(Assembly assembly)
        {
            return Assembly(assembly);
        }

        public IIncludedOnlyAssemblies IncludingOnly
        {
            get { return Bootstrapper.IncludingOnly; }
        }

        public IExcludedAssemblies Excluding
        {
            get { return Bootstrapper.Excluding; }
        }

        public BootstrapperExtensions With
        {
            get { return Bootstrapper.With; }
        }

        public void Start()
        {
            Bootstrapper.Start();
        }
    }
}
