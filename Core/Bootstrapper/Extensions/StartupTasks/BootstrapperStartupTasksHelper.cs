namespace Bootstrap.Extensions.StartupTasks
{
    public static class BootstrapperStartupTasksHelper
    {
        public static StartupTasksOptions StartupTasks(this BootstrapperExtensions extensions)
        {
            var extension = new StartupTasksExtension();
            extensions.Extension(extension);
            return extension.Options;
        }
    }
}
