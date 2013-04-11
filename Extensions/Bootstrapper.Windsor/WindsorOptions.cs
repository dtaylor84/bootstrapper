using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Castle.Windsor;

namespace Bootstrap.Windsor
{
    public class WindsorOptions : IBootstrapperContainerExtensionOptions
    {
        private readonly IBootstrapperContainerExtensionOptions options;

        public IWindsorContainer Container { get; set; }
        public BootstrapperExtensions And { get { return options.And; } }
        public bool AutoRegistration { get { return options.AutoRegistration; } }

        public WindsorOptions(IBootstrapperContainerExtensionOptions options)
        {
            this.options = options;
        }

        public WindsorOptions WithContainer(IWindsorContainer container)
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
