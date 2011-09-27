using System.Collections.Generic;
using System.Reflection;
using Bootstrap.Extensions;

namespace Bootstrap
{
    public class IncludedOnlyAssemblies: IIncludedOnlyAssemblies
    {
        public List<Assembly> Assemblies { get; set; }

        public IncludedOnlyAssemblies()
        {
            Assemblies = new List<Assembly>();
        }

        public IIncludedOnlyAssemblies Assembly(Assembly assembly)
        {
            Assemblies.Add(assembly);
            return this;
        }

        public IIncludedOnlyAssemblies AndAssembly(Assembly assembly)
        {
            return Assembly(assembly);
        }

        public IIncludedAssemblies Including
        {
            get { return Bootstrapper.Including; }
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
