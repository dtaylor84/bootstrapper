using Bootstrap.Extensions.Containers;

namespace Bootstrap.Extensions.StartupTasks
{
    public static class BootstrapperStartupTasksHelper
    {
        public static StartupTasksOptions StartupTasks(this BootstrapperExtensions extensions)
        {
            var extension = new StartupTasksExtension(new RegistrationHelper());
            extensions.Extension(extension);
            return extension.Options;
        }
    }
}
