using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.Unity
{
    public static class BootstrapperUnityHelper
    {
        public static UnityOptions Unity(this BootstrapperExtensions extensions)
        {
            var extension = new UnityExtension(new RegistrationHelper(), new BootstrapperContainerExtensionOptions());
            extensions.Extension(extension);
            return extension.Options;
        }
    }
}
