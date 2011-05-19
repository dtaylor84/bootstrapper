using Bootstrap.Extensions.Containers;

namespace Bootstrap.Extensions.StartupTasks
{
    public class StartupTasksRegistration: IBootstrapperRegistration
    {
        public void Register(IBootstrapperContainerExtension containerExtension)
        {
            containerExtension.RegisterAll<IStartupTask>();
        }
    }
}
