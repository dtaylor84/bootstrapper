using Bootstrap.Extensions;

namespace Bootstrap.StartupTasks
{
    public static class BootstrapperStartupTasksHelper
    {
        public static BootstrapperExtensions StartupTasks(this BootstrapperExtensions extensions)
        {
            return extensions.Extension(new StartupTasksExtension());
        }
    }
}
