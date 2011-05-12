namespace Bootstrap.Extensions.Containers
{
    public class BootstrapperContainerExtensionOptions: BootstrapperExtensionOptions, IBootstrapperContainerExtensionOptions
    {
        public bool UseAutoRegistration { get; private set; }

        public IBootstrapperExtensionOptions WithAutoRegistration()
        {
            UseAutoRegistration = true;
            return this;
        }
    }
}
