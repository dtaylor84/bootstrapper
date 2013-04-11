using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.Autofac
{
    public static class BootstrapperAutofacHelper
    {
        public static AutofacOptions Autofac(this BootstrapperExtensions extensions)
        {
            var extension = new AutofacExtension(new RegistrationHelper(), new BootstrapperContainerExtensionOptions());
            extensions.Extension(extension);
            return extension.Options;
        }
    }
}
