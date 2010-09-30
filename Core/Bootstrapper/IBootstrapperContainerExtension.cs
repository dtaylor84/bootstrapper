namespace Bootstrap
{
    public interface IBootstrapperContainerExtension: IBootstrapperExtension
    {
        IAssemblyCollector LookForRegistrations { get; }
        IAssemblyCollector LookForMaps { get; }
        IAssemblyCollector LookForStartupTasks { get; }
    }
}
