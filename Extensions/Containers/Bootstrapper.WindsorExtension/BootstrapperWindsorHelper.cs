using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.Windsor
{
    public static class BootstrapperWindsorHelper
    {
        public static IBootstrapperContainerExtensionOptions Windsor(this BootstrapperExtensions extensions)
        {
            var extension = new WindsorExtension();
            extensions.Extension(extension);
            return extension.Options;
        }
    }
}
