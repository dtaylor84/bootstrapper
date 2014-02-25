using Bootstrap.Extensions.Containers;
using SimpleInjector;

namespace Bootstrap.SimpleInjector
{
    public class SimpleInjectorOptions : BootstrapperOption, IBootstrapperContainerExtensionOptions
    {
        private readonly IBootstrapperContainerExtensionOptions options;

        public Container Container { get; set; }
        public bool AutoRegistration { get { return options.AutoRegistration; } }

        public SimpleInjectorOptions(IBootstrapperContainerExtensionOptions options)
        {
            this.options = options;
        }

        public SimpleInjectorOptions WithContainer(Container container)
        {
            Container = container;
            return this;
        }

        public IBootstrapperOption UsingAutoRegistration()
        {
            options.UsingAutoRegistration();
            return this;
        }
    }
}
