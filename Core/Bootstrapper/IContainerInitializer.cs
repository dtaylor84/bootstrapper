namespace Bootstrapper
{
    public interface IContainerInitializer
    {
        IContainerInitializerOptions WithOptions { get; }
        object Initialize();
    }
}
