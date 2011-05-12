namespace Bootstrap.Extensions.Containers
{
    public interface IBootstrapperContainerExtensionOptions: IBootstrapperExtensionOptions
    {
        IBootstrapperExtensionOptions WithAutoRegistration();
        bool UseAutoRegistration { get;}
    }
}
