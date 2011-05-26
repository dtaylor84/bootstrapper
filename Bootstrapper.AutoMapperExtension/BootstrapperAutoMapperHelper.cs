using Bootstrap.Extensions;

namespace Bootstrap.AutoMapper
{
    public static class BootstrapperAutoMapperHelper
    {
        public static BootstrapperExtensions AutoMapper(this BootstrapperExtensions extensions)
        {
            return extensions.Extension(new AutoMapperExtension());
        }

    }
}
