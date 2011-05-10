using Bootstrap.Extensions;

namespace Bootstrap.StructureMap
{
    public static class BootstrapperStructureMapHelper
    {
        public static BootstrapperExtensions StructureMap(this BootstrapperExtensions extensions)
        {
            return extensions.Extension(new StructureMapExtension());
        }
    }
}
