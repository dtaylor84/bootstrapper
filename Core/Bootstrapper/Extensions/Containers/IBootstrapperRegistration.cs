namespace Bootstrap.Extensions.Containers
{
    public interface IBootstrapperRegistration
    {
        void Register(IBootstrapperContainerExtension containerExtension);
    }
}
