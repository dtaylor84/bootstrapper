using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Ninject;

namespace Bootstrap.Ninject
{
    public class NinjectOptions : IBootstrapperContainerExtensionOptions
    {
        private readonly IBootstrapperContainerExtensionOptions options;

        public IKernel Container { get; set; }
        public BootstrapperExtensions And { get { return options.And; } }
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
