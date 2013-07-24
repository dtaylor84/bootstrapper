using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.SimpleInjector
{
    public static class SimpleInjectorConvenienceExtensions
    {
        public static SimpleInjectorOptions SimpleInjector(this BootstrapperExtensions extensions)
        {
            var extension = new SimpleInjectorExtension(Bootstrapper.RegistrationHelper, new BootstrapperContainerExtensionOptions());
            extensions.Extension(extension);
            return extension.Options;
        }

    }
}
