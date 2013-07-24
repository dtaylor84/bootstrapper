using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.StructureMap
{
    public static class StructureMapConvenienceExtensions
    {
        public static StructureMapOptions StructureMap(this BootstrapperExtensions extensions)
        {
            var extension = new StructureMapExtension(Bootstrapper.RegistrationHelper, new BootstrapperContainerExtensionOptions());
            extensions.Extension(extension);
            return extension.Options;
        }
    }
}
