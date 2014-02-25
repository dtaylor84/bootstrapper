using System.Collections.Generic;

namespace Bootstrap
{
    public class ExcludedAssemblies: BootstrapperOption, IExcludedAssemblies
    {
        public List<string> Assemblies {get; set; }

        public ExcludedAssemblies()
        {
            Assemblies = new List<string>{"System", "mscorlib", "Microsoft.VisualStudio"};
        }

        public IExcludedAssemblies Assembly(string assembly)
        {
            Assemblies.Add(assembly);
            return this;
        }

        public IExcludedAssemblies AndAssembly(string assembly)
        {
            return Assembly(assembly);
        }
    }
}
