using Autofac;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.Autofac
{
    public class AutofacOptions: BootstrapperOption, IBootstrapperContainerExtensionOptions
    {
        private readonly IBootstrapperContainerExtensionOptions options;

        public IContainer Container { get; set; }
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

        public IBootstrapperOption UsingAutoRegistration()
        {
            options.UsingAutoRegistration();
            return this;
        }
    }
}