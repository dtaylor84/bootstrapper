using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.Autofac
{
    public static class AutofacConvenienceExtensions
    {
        public static AutofacOptions Autofac(this BootstrapperExtensions extensions)
        {
            var extension = new AutofacExtension(Bootstrapper.RegistrationHelper, new BootstrapperContainerExtensionOptions());
            extensions.Extension(extension);
            return extension.Options;
        }
    }
}
