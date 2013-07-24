namespace Bootstrap.Extensions.Containers
{
    public class BootstrapperContainerExtensionOptions: BootstrapperOption, IBootstrapperContainerExtensionOptions
    {
        public bool AutoRegistration { get; private set; }

        public IBootstrapperOption UsingAutoRegistration()
        {
            AutoRegistration = true;
            return this;
        }
    }
}
