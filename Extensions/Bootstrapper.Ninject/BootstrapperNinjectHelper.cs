using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.Ninject
{
    public static class BootstrapperNinjectHelper
    {
        public static NinjectOptions Ninject(this BootstrapperExtensions extensions)
        {
            var extension = new NinjectExtension(new RegistrationHelper(), new BootstrapperContainerExtensionOptions());
            extensions.Extension(extension);
            return extension.Options;
        }
    }
}
