using System.Linq;
using Bootstrap.Extensions;

namespace Bootstrap.StartupTasks
{
    public class StartupTasksExtension:IBootstrapperExtension
    {
        public void Run()
        {
            var container = Bootstrapper.ContainerExtension;
            container.ResolveAll<IStartupTask>().ToList().ForEach(t => t.Run());
        }

        public void Reset()
        {
            var container = Bootstrapper.ContainerExtension;
            container.ResolveAll<IStartupTask>().ToList().ForEach(t => t.Reset());
        }
    }
}
