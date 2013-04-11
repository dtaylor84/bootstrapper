using Autofac;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.Autofac
{
    public class AutofacOptions: IBootstrapperContainerExtensionOptions
    {
        private readonly IBootstrapperContainerExtensionOptions options;

        public IContainer Container { get; set; }
        public BootstrapperExtensions And {get { return options.And; }}
        public bool AutoRegistration {get { return options.AutoRegistration; }}

        public AutofacOptions(IBootstrapperContainerExtensionOptions options)
        {
            this.options = options;
        }
       
        public AutofacOptions WithContainer(IContainer container)
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