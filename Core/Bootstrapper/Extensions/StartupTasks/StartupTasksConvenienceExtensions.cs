namespace Bootstrap.Extensions.StartupTasks
{
    public static class StartupTasksConvenienceExtensions
    {
        public static StartupTasksOptions StartupTasks(this BootstrapperExtensions extensions)
        {
            var extension = new StartupTasksExtension(Bootstrapper.RegistrationHelper);
            extensions.Extension(extension);
            return extension.Options;
        }
    }
}
