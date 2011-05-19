using System.Collections;
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
        public StartupTasksOptions Options { get; private set; }

        public StartupTasksExtension()
        {
            Options = new StartupTasksOptions();
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

        private Dictionary<IStartupTask, int> AddSequencePosition(List<IStartupTask> tasks)
        {
            var sortedTasks = new Dictionary<IStartupTask, int>();
            tasks.ForEach(t => sortedTasks.Add(t, GetSequencePosition(t, tasks)));
            return sortedTasks;
        }

        private int GetSequencePosition(IStartupTask task, ICollection tasks)
        {
            return GetFluentlyDeclaredPosition(task, tasks) ??
                   GetAttributePosition(task) ??
                   GetRestPosition(tasks) ??
                   DefaultPosition;
        }

        private int? GetFluentlyDeclaredPosition(IStartupTask task, ICollection tasks)
        {
            var sequence = Options.Sequence;
            if (!sequence.Contains(task.GetType())) return null;
            if (!sequence.Contains(typeof(IStartupTask))) return sequence.IndexOf(tasks.GetType()) + 1;
            if (sequence.IndexOf(typeof(IStartupTask)) > sequence.IndexOf(task.GetType())) return sequence.IndexOf(task.GetType()) + 1;
            return tasks.Count + sequence.IndexOf(task.GetType()) - sequence.IndexOf(typeof(IStartupTask));
        }

        private static int? GetAttributePosition(IStartupTask task)
        {
            var attribute = task.GetType().GetCustomAttributes(false).FirstOrDefault(a => a is TaskAttribute);
            if (attribute == null) return null;
            return ((TaskAttribute)attribute).PositionInSequence;            
        }

        private int? GetRestPosition(ICollection tasks)
        {
            var sequence = Options.Sequence;
            if (!sequence.Contains(typeof(IStartupTask))) return null;
            return tasks.Count;
        }

    }
}
