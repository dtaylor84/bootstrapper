namespace Bootstrap.Extensions.Containers
{
    public interface IBootstrapperContainerExtensionOptions: IBootstrapperExtensionOptions
    {
        IBootstrapperContainerExtensionOptions UsingAutoRegistration();
        bool AutoRegistration { get;}
    }
}
