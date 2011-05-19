using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.Extensions.StartupTasks
{
    public class StartupTasksExtension:IBootstrapperExtension
    {
        const int DefaultPosition = int.MaxValue;
        public List<ExecutionLogEntry> ExecutionLog { get; private set; }
        public StartupTasksOptions Options { get; private set; }

        public StartupTasksExtension()
        {
            Options = new StartupTasksOptions();
            ExecutionLog = new List<ExecutionLogEntry>();
        }

        public void Run()
        {
            List<IStartupTask> tasks;

            if (Bootstrapper.ContainerExtension != null && Bootstrapper.Container != null)
                tasks = Bootstrapper.ContainerExtension.ResolveAll<IStartupTask>().ToList();
            else
                tasks = RegistrationHelper.GetInstancesOfTypesImplementing<IStartupTask>();
            
            AddSequencePosition(tasks)
                .OrderBy(p => p.Position).ToList()
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
                .OrderByDescending(p => p.Position).ToList()
                .ForEach(Reset);
        }

        private void Run(TaskExecutionParameters taskExecutionParameters)
        {
            var logEntry = new ExecutionLogEntry
                               {
                                   Timestamp = DateTime.Now,
                                   TaskName = "+" + taskExecutionParameters.Task.GetType().Name,
                                   SequencePosition = taskExecutionParameters.Position,
                                   DelayInMilliseconds = taskExecutionParameters.Delay
                               };
            if (taskExecutionParameters.Delay > 0) Thread.Sleep(taskExecutionParameters.Delay);
            logEntry.StartedAt = DateTime.Now;
            taskExecutionParameters.Task.Run();
            logEntry.EndedAt = DateTime.Now;
            ExecutionLog.Add(logEntry);
        }

        private void Reset(TaskExecutionParameters taskExecutionParameters)
        {
            var logEntry = new ExecutionLogEntry
                               {
                                   Timestamp = DateTime.Now,
                                   TaskName = "-" + taskExecutionParameters.Task.GetType().Name,
                                   SequencePosition = taskExecutionParameters.Position,
                                   DelayInMilliseconds = 0,
                                   StartedAt = DateTime.Now
                               };
            taskExecutionParameters.Task.Reset();
            logEntry.EndedAt = DateTime.Now;
            ExecutionLog.Add(logEntry);
        }

        private IEnumerable<TaskExecutionParameters> AddSequencePosition(List<IStartupTask> tasks)
        {
            var sortedTasks = new List<TaskExecutionParameters>();
            tasks.ForEach(t => sortedTasks.Add(new TaskExecutionParameters
                                                   {
                                                       Task = t,
                                                       Position = GetSequencePosition(t, tasks),
                                                       Delay = GetDelay(t)
                                                   }));
            return AdjustDelayForTheRest(sortedTasks);
        }

        private IEnumerable<TaskExecutionParameters> AdjustDelayForTheRest(IEnumerable<TaskExecutionParameters> tasks)
        {
            var theRest = Options.Sequence.FirstOrDefault(t => t.TaskType == typeof(IStartupTask));
            if (theRest == null) return tasks;

            tasks = tasks.OrderBy(t => t.Position);

            var match = tasks.FirstOrDefault(t => t.Delay == -1);
            if (match == null) return tasks;

            match.Delay = theRest.Delay;
            tasks.Where(t => t.Delay == -1).ToList().ForEach(t => t.Delay = 0);

            return tasks;
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
            var sequence = Options.Sequence.Select(s => s.TaskType).ToList();
            if (!sequence.Contains(task.GetType())) return null;
            if (!sequence.Contains(typeof(IStartupTask))) return sequence.IndexOf(tasks.GetType()) + 1;
            if (sequence.IndexOf(typeof(IStartupTask)) > sequence.IndexOf(task.GetType())) return sequence.IndexOf(task.GetType()) + 1;
            return tasks.Count + sequence.IndexOf(task.GetType()) - sequence.IndexOf(typeof(IStartupTask));
        }

        private static int? GetAttributePosition(IStartupTask task)
        {
            var attribute = task.GetType().GetCustomAttributes(false).FirstOrDefault(a => a is TaskAttribute) as TaskAttribute;
            if (attribute == null) return null;
            if (attribute.PositionInSequence == int.MaxValue) return null;
            return attribute.PositionInSequence;            
        }

        private int? GetRestPosition(ICollection tasks)
        {
            var sequence = Options.Sequence.Select(s => s.TaskType).ToList();
            if (!sequence.Contains(typeof(IStartupTask))) return null;
            return tasks.Count;
        }

        private int GetDelay(IStartupTask task)
        {
            return  GetFluentlyDeclaredDelay(task) ??
                    GetAttributeDelay(task) ??
                    GetRestDelay() ?? 
                    0;
        }

        private int? GetFluentlyDeclaredDelay(IStartupTask task)
        {
            var match = Options.Sequence.FirstOrDefault(t => t.TaskType == task.GetType());
            if (match == null) return null;
            if (match.Delay == 0) return null;
            return match.Delay;
        }

        private static int? GetAttributeDelay(IStartupTask task)
        {
            var attribute = task.GetType().GetCustomAttributes(false).FirstOrDefault(a => a is TaskAttribute) as TaskAttribute;
            if (attribute == null) return null;
            if (attribute.DelayStartBy == 0) return null;
            return attribute.DelayStartBy;
        }

        public int? GetRestDelay()
        {
            var match = Options.Sequence.FirstOrDefault(t => t.TaskType == typeof(IStartupTask));
            if (match == null) return null;
            return -1;            
        }
    }
}
