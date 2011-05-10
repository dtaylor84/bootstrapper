using Bootstrap.Extensions;

namespace Bootstrap.ServiceLocator
{
    public static class BootstrapperServiceLocatorHelper
    {
        public static BootstrapperExtensions ServiceLocator(this BootstrapperExtensions extensions)
        {
            return extensions.Extension(new ServiceLocatorExtension());
        }
    }
}
