using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.Unity
{
    public static class UnityConvenienceExtensions
    {
        public static UnityOptions Unity(this BootstrapperExtensions extensions)
        {
            var extension = new UnityExtension(Bootstrapper.RegistrationHelper, new BootstrapperContainerExtensionOptions());
            extensions.Extension(extension);
            return extension.Options;
        }
    }
}
