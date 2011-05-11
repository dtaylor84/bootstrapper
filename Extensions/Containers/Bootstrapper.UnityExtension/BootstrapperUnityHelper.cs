using Bootstrap.Extensions;

namespace Bootstrap.Unity
{
    public static class BootstrapperUnityHelper
    {
        public static BootstrapperExtensions Unity(this BootstrapperExtensions extensions)
        {
            return extensions.Extension(new UnityExtension());
        }
    }
}
