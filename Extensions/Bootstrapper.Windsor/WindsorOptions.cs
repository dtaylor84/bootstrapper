using Bootstrap.Extensions.Containers;
using Castle.Windsor;

namespace Bootstrap.Windsor
{
    public class WindsorOptions : BootstrapperOption, IBootstrapperContainerExtensionOptions
    {
        private readonly IBootstrapperContainerExtensionOptions options;

        public IWindsorContainer Container { get; set; }
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

        public IBootstrapperOption UsingAutoRegistration()
        {
            options.UsingAutoRegistration();
            return this;
        }
    }
}
