namespace Bootstrap.Extensions.Containers
{
    public class BootstrapperContainerExtensionOptions: BootstrapperExtensionOptions, IBootstrapperContainerExtensionOptions
    {
        public bool AutoRegistration { get; private set; }

        public IBootstrapperContainerExtensionOptions UsingAutoRegistration()
        {
            AutoRegistration = true;
            return this;
        }
    }
}
