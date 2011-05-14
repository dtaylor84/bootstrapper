namespace Bootstrap.Extensions
{
    public interface IBootstrapperExtensionOptions
    {
        BootstrapperExtensions And { get; }
        void Start();
    }
}
