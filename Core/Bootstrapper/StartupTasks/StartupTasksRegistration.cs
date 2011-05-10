using Bootstrap.Extensions.Containers;

namespace Bootstrap.StartupTasks
{
    public class StartupTasksRegistration: IBootstrapperRegistration
    {
        public void Register(IBootstrapperContainerExtension container)
        {
            container.RegisterAll<IStartupTask>();
        }
    }
}
