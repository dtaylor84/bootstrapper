using Bootstrap.Extensions;

namespace Bootstrap.Windsor
{
    public static class BootstrapperWindsorHelper
    {
        public static BootstrapperExtensions Windsor(this BootstrapperExtensions extensions)
        {
            return extensions.Extension(new WindsorExtension());
        }
    }
}
