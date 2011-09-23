using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.Autofac
{
    public static class BootstrapperAutofacHelper
    {
        public static IBootstrapperContainerExtensionOptions Autofac(this BootstrapperExtensions extensions)
        {
            var extension = new AutofacExtension();
            extensions.Extension(extension);
            return extension.Options;
        }
    }
}
