using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.SimpleInjector
{
    public static class BootstrapperSimpleInjectorHelper
    {
        public static SimpleInjectorOptions SimpleInjector(this BootstrapperExtensions extensions)
        {
            var extension = new SimpleInjectorExtension(new RegistrationHelper(), new BootstrapperContainerExtensionOptions());
            extensions.Extension(extension);
            return extension.Options;
        }

    }
}
