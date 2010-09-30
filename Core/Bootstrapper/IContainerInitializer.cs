namespace Bootstrap
{
    public interface IContainerInitializer
    {
        IContainerInitializerOptions WithOptions { get; }
        object Initialize();
    }
}
