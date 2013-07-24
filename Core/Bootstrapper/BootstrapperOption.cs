using Bootstrap.Extensions;

namespace Bootstrap
{
    public class BootstrapperOption: IBootstrapperOption
    {
        public BootstrapperExtensions With { get { return Bootstrapper.With; } }
        public BootstrapperExtensions And { get { return Bootstrapper.With; } }
        public IExcludedAssemblies Excluding { get { return Bootstrapper.Excluding; } }
        public IIncludedAssemblies Including { get { return Bootstrapper.Including; } }
        public IIncludedOnlyAssemblies IncludingOnly { get { return Bootstrapper.IncludingOnly; } }
        public IAssemblySetOptions LookForTypesIn { get { return Bootstrapper.LookForTypesIn; } }
        public void Start() { Bootstrapper.Start();}
    }
}