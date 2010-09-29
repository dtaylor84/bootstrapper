namespace Bootstrapper
{
    public interface IContainerInitializerOptions
    {
        IAssemblyCollector LookForRegistrations { get; }
        object Initialize();
    }
}
