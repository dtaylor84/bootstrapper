using System;
using System.Collections.Generic;
using Bootstrap.Extensions;

namespace Bootstrap
{
    public class ExcludedAssemblies: IExcludedAssemblies
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

        public BootstrapperExtensions With
        {
            get { return Bootstrapper.With; }
        }

        public IIncludedAssemblies Including
        {
            get { return Bootstrapper.Including; }
        }

        public void Start()
        {
            Bootstrapper.Start();
        }
    }
}
