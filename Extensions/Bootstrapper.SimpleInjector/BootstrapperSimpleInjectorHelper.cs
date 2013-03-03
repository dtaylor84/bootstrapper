using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.SimpleInjector
{
    public static class BootstrapperSimpleInjectorHelper
    {
        public static IBootstrapperContainerExtensionOptions SimpleInjector(this BootstrapperExtensions extensions)
        {
            var extension = new SimpleInjectorExtension(new RegistrationHelper());
            extensions.Extension(extension);
            return extension.Options;
        }

    }
}
