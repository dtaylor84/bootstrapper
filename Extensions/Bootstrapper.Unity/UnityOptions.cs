using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Microsoft.Practices.Unity;

namespace Bootstrap.Unity
{
    public class UnityOptions : IBootstrapperContainerExtensionOptions
    {
        private readonly IBootstrapperContainerExtensionOptions options;

        public IUnityContainer Container { get; set; }
        public BootstrapperExtensions And { get { return options.And; } }
        public bool AutoRegistration { get { return options.AutoRegistration; } }

        public UnityOptions(IBootstrapperContainerExtensionOptions options)
        {
            this.options = options;
        }

        public UnityOptions WithContainer(IUnityContainer container)
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
