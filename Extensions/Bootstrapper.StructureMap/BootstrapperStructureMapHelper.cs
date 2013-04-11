using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.StructureMap
{
    public static class BootstrapperStructureMapHelper
    {
        public static IBootstrapperContainerExtensionOptions StructureMap(this BootstrapperExtensions extensions)
        {
            var extension = new StructureMapExtension(new RegistrationHelper(), new BootstrapperContainerExtensionOptions());
            extensions.Extension(extension);
            return extension.Options;
        }
    }
}
