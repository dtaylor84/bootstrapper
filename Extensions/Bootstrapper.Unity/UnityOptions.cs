using Bootstrap.Extensions.Containers;
using Microsoft.Practices.Unity;

namespace Bootstrap.Unity
{
    public class UnityOptions : BootstrapperOption, IBootstrapperContainerExtensionOptions
    {
        private readonly IBootstrapperContainerExtensionOptions options;

        public IUnityContainer Container { get; set; }
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

        public IBootstrapperOption UsingAutoRegistration()
        {
            options.UsingAutoRegistration();
            return this;
        }
    }
}
