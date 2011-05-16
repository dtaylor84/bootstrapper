using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.StartupTasks
{
    public class StartupTasksExtension:IBootstrapperExtension
    {
        public void Run()
        {
            List<IStartupTask> tasks;

            if (Bootstrapper.ContainerExtension != null && Bootstrapper.Container != null)
                tasks = Bootstrapper.ContainerExtension.ResolveAll<IStartupTask>().ToList();
            else
                tasks = RegistrationHelper.GetInstancesOfTypesImplementing<IStartupTask>();
            tasks.ForEach(t => t.Run());
        }

        public void Reset()
        {
            List<IStartupTask> tasks;

            if (Bootstrapper.ContainerExtension != null && Bootstrapper.Container != null)
                tasks = Bootstrapper.ContainerExtension.ResolveAll<IStartupTask>().ToList();
            else
                tasks = RegistrationHelper.GetInstancesOfTypesImplementing<IStartupTask>();
            tasks.ForEach(t => t.Reset());
        }
    }
}
