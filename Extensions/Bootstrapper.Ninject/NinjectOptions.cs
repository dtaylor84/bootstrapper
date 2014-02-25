using Bootstrap.Extensions.Containers;
using Ninject;

namespace Bootstrap.Ninject
{
    public class NinjectOptions : BootstrapperOption, IBootstrapperContainerExtensionOptions
    {
        private readonly IBootstrapperContainerExtensionOptions options;

        public IKernel Container { get; set; }
        public bool AutoRegistration { get { return options.AutoRegistration; } }

        public NinjectOptions(IBootstrapperContainerExtensionOptions options)
        {
            this.options = options;
        }

        public NinjectOptions WithContainer(IKernel container)
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
