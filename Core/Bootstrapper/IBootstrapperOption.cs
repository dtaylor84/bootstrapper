using Bootstrap.Extensions;

namespace Bootstrap
{
    public interface IBootstrapperOption
    {
        BootstrapperExtensions With { get; }
        BootstrapperExtensions And { get; }
        IExcludedAssemblies Excluding { get; }
        IIncludedAssemblies Including { get; }
        IIncludedOnlyAssemblies IncludingOnly { get; }
        IAssemblySetOptions LookForTypesIn { get; }
        void Start();
    }
}