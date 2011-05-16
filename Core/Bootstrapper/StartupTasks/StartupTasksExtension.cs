using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.StartupTasks
{
    public class StartupTasksExtension:IBootstrapperExtension
    {
        const int DefaultPosition = int.MaxValue;
        public List<string> ExecutionLog { get; private set; }

        public StartupTasksExtension()
        {
            ExecutionLog = new List<string>();
        }

        public void Run()
        {
            List<IStartupTask> tasks;

            if (Bootstrapper.ContainerExtension != null && Bootstrapper.Container != null)
                tasks = Bootstrapper.ContainerExtension.ResolveAll<IStartupTask>().ToList();
            else
                tasks = RegistrationHelper.GetInstancesOfTypesImplementing<IStartupTask>();
            
            AddSequencePosition(tasks)
                .OrderBy(p => p.Value)
                .Select(p => p.Key).ToList()
                .ForEach(Run);
        }

        public void Reset()
        {
            List<IStartupTask> tasks;

            if (Bootstrapper.ContainerExtension != null && Bootstrapper.Container != null)
                tasks = Bootstrapper.ContainerExtension.ResolveAll<IStartupTask>().ToList();
            else
                tasks = RegistrationHelper.GetInstancesOfTypesImplementing<IStartupTask>();

            AddSequencePosition(tasks)
                .OrderByDescending(p => p.Value)
                .Select(p => p.Key).ToList()
                .ForEach(Reset);
        }

        private void Run(IStartupTask task)
        {
            task.Run();
            ExecutionLog.Add("+" + task.GetType().Name);
        }

        private void Reset(IStartupTask task)
        {
            task.Reset();
            ExecutionLog.Add("-" + task.GetType().Name);
        }

        private static Dictionary<IStartupTask, int> AddSequencePosition(List<IStartupTask> tasks)
        {
            var sortedTasks = new Dictionary<IStartupTask, int>();
            tasks.ForEach(t => sortedTasks.Add(t, GetSequencePosition(t)));
            return sortedTasks;
        }

        private static int GetSequencePosition(IStartupTask task)
        {
            var attribute = task.GetType().GetCustomAttributes(false).FirstOrDefault(a => a is TaskAttribute);
            return attribute == null ? DefaultPosition : ((TaskAttribute)attribute).PositionInSequence;
        }

    }
}
