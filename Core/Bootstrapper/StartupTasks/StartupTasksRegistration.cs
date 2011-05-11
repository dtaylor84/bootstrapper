using Bootstrap.Extensions.Containers;

namespace Bootstrap.StartupTasks
{
    public class StartupTasksRegistration: IBootstrapperRegistration
    {
        public void Register(IBootstrapperContainerExtension containerExtension)
        {
            containerExtension.RegisterAll<IStartupTask>();
        }
    }
}
