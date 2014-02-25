namespace Bootstrap.Extensions.Containers
{
    public interface IBootstrapperContainerExtensionOptions
    {
        IBootstrapperOption UsingAutoRegistration();
        bool AutoRegistration { get;}
    }
}
