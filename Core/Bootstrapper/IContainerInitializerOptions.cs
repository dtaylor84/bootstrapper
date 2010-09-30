namespace Bootstrap
{
    public interface IContainerInitializerOptions
    {
        IAssemblyCollector LookForRegistrations { get; }
        object Initialize();
    }
}
