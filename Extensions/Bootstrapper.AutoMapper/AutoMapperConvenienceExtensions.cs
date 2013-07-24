using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.AutoMapper
{
    public static class AutoMapperConvenienceExtensions
    {
        public static BootstrapperExtensions AutoMapper(this BootstrapperExtensions extensions)
        {
            return extensions.Extension(new AutoMapperExtension(new RegistrationHelper(null)));
        }

    }
}
