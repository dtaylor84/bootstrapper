using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using StructureMap;

namespace Bootstrap.StructureMap
{
    public class StructureMapOptions : IBootstrapperContainerExtensionOptions
    {
        private readonly IBootstrapperContainerExtensionOptions options;

        public IContainer Container { get; set; }
        public BootstrapperExtensions And { get { return options.And; } }
        public bool AutoRegistration { get { return options.AutoRegistration; } }

        public StructureMapOptions(IBootstrapperContainerExtensionOptions options)
        {
            this.options = options;
        }

        public StructureMapOptions WithContainer(IContainer container)
        {
            Container = container;
            return this;
        }

        public StructureMapOptions UsingObjectFactory()
        {
            WithContainer(ObjectFactory.Container);
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
