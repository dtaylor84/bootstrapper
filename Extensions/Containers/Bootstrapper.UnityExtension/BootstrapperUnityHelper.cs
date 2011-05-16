using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.Unity
{
    public static class BootstrapperUnityHelper
    {
        public static IBootstrapperContainerExtensionOptions Unity(this BootstrapperExtensions extensions)
        {
            var extension = new UnityExtension();
            extensions.Extension(extension);
            return extension.Options;
        }
    }
}
