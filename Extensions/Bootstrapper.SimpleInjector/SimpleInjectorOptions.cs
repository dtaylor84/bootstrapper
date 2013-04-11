using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using SimpleInjector;

namespace Bootstrap.SimpleInjector
{
    public class SimpleInjectorOptions : IBootstrapperContainerExtensionOptions
    {
        private readonly IBootstrapperContainerExtensionOptions options;

        public Container Container { get; set; }
        public BootstrapperExtensions And { get { return options.And; } }
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

        public void Start()
        {
            options.Start();
        }

        public IBootstrapperContainerExtensionOptions UsingAutoRegistration()
        {
            options.UsingAutoRegistration();
            return this;
        }
    }
}
