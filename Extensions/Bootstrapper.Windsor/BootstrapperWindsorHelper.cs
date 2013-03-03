using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Castle.Core.Internal;
using Castle.MicroKernel;

namespace Bootstrap.Windsor
{
    public static class BootstrapperWindsorHelper
    {
        public static IBootstrapperContainerExtensionOptions Windsor(this BootstrapperExtensions extensions, params IFacility[] facilities)
        {
            var extension = new WindsorExtension(new RegistrationHelper());
            facilities.ForEach(extension.AddFacility);
            extensions.Extension(extension);
            return extension.Options;
        }
    }
}
