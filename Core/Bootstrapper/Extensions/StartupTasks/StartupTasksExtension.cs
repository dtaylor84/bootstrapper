﻿using System;
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
        public StartupTasksOptions Options { get; private set; }
        private readonly List<TaskGroup> taskGroups;
        internal readonly IRegistrationHelper Registrator;

        public List<ExecutionLogEntry> ExecutionLog { 
            get
            {
                var log = new List<ExecutionLogEntry>();
                taskGroups.ForEach(g => log.AddRange(g.ExecutionLog));
                return log;
            }
        }

        public StartupTasksExtension(IRegistrationHelper registrationHelper)
        {
            Options = new StartupTasksOptions();
            taskGroups = new List<TaskGroup>();
            Registrator = registrationHelper;
        }

        public void Run()
        {
            BuildTaskGroups(GetTasks());

            if (taskGroups.Count == 1) RunGroup(taskGroups[0]);
            else
            {
                taskGroups.ForEach(g => g.Thread = new Thread(() => RunGroup(g)));
                taskGroups.ForEach(g => g.Thread.Start());
                taskGroups.ForEach(g => g.Thread.Join());
            }
        }

        public void Reset()
        {
            BuildTaskGroups(GetTasks());
            taskGroups.Reverse();

            taskGroups.ForEach(g => 
                g.Tasks.OrderByDescending(t => t.Position).ToList()
                    .ForEach(t => Reset(t,g)));
        }

        public void RunGroup(TaskGroup group)
        {
            group.Tasks.ForEach(t => Run(t, group));
        }

        private static void Run(TaskExecutionParameters taskExecutionParameters, TaskGroup group)
        {
            var logEntry = new ExecutionLogEntry
                               {
                                   Timestamp = DateTime.Now,
                                   TaskName = "+" + taskExecutionParameters.Task.GetType().Name,
                                   Group = taskExecutionParameters.Group,
                                   SequencePosition = taskExecutionParameters.Position,
                                   DelayInMilliseconds = taskExecutionParameters.Delay
                               };
            if (taskExecutionParameters.Delay > 0) Thread.Sleep(taskExecutionParameters.Delay);
            logEntry.StartedAt = DateTime.Now;
            taskExecutionParameters.Task.Run();
            logEntry.EndedAt = DateTime.Now;
            group.ExecutionLog.Add(logEntry);
        }

        private static void Reset(TaskExecutionParameters taskExecutionParameters, TaskGroup group)
        {
            var logEntry = new ExecutionLogEntry
                               {
                                   Timestamp = DateTime.Now,
                                   TaskName = "-" + taskExecutionParameters.Task.GetType().Name,
                                   Group = taskExecutionParameters.Group,
                                   SequencePosition = taskExecutionParameters.Position,
                                   DelayInMilliseconds = 0,
                                   StartedAt = DateTime.Now
                               };
            taskExecutionParameters.Task.Reset();
            logEntry.EndedAt = DateTime.Now;
            group.ExecutionLog.Add(logEntry);
        }

        private List<IStartupTask> GetTasks()
        {
            List<IStartupTask> tasks;
            if (Bootstrapper.ContainerExtension != null && Bootstrapper.Container != null)
                tasks = Bootstrapper.ContainerExtension.ResolveAll<IStartupTask>().OrderBy(t => t.GetType().Name).ToList();
            else
                tasks = Registrator.GetInstancesOfTypesImplementing<IStartupTask>().OrderBy(t => t.GetType().Name).ToList();
            return tasks;
        }

        private void BuildTaskGroups(List<IStartupTask> tasks)
        {
            taskGroups.Clear();
            AddExecutionParameters(tasks)
                .OrderBy(t => t.Position)
                .GroupBy(t => t.Group)
                .ToList().ForEach(g => taskGroups.Add(new TaskGroup { Tasks = g.ToList(), ExecutionLog = new List<ExecutionLogEntry>() }));
        }

        private IEnumerable<TaskExecutionParameters> AddExecutionParameters(List<IStartupTask> tasks)
        {
            var tasksWithParameters = new List<TaskExecutionParameters>();
            tasks.ForEach(t => tasksWithParameters.Add(new TaskExecutionParameters
                                                   {
                                                       Task = t,
                                                       Position = GetSequencePosition(t, tasks),
                                                       Delay = GetDelay(t),
                                                       Group = GetGroup(t)
                                                   }));
            return AdjustDelayForTheRest(tasksWithParameters);
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
            var group = Options.Groups.FirstOrDefault(g => g.Sequence.Any(t => t.TaskType == task.GetType()));
            if (group == null) return null;
            var sequence = group.Sequence.Select(s => s.TaskType).ToList();
            if (!sequence.Contains(typeof(IStartupTask))) return sequence.IndexOf(task.GetType()) + 1;
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
            var group = Options.Groups.FirstOrDefault(g => g.Sequence.Any(t => t.TaskType == typeof(IStartupTask)));
            if (group == null) return null;
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
            var group = Options.Groups.FirstOrDefault(g => g.Sequence.Any(t => t.TaskType == task.GetType()));
            if (group == null) return null;
            var match = group.Sequence.FirstOrDefault(t => t.TaskType == task.GetType());
            if (match == null || match.Delay == 0) return null;
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

        private int GetGroup(IStartupTask task)
        {
            return  GetFluentlyDeclaredGroup(task) ??
                    GetAttributeGroup(task) ??
                    GetRestGroup() ??
                    0;
        }

        private int? GetFluentlyDeclaredGroup(IStartupTask task)
        {
            var group = Options.Groups.FirstOrDefault(g => g.Sequence.Any(t => t.TaskType == task.GetType()));
            if (group == null) return null;
            return Options.Groups.IndexOf(group);
        }

        private static int? GetAttributeGroup(IStartupTask task)
        {
            var attribute = task.GetType().GetCustomAttributes(false).FirstOrDefault(a => a is TaskAttribute) as TaskAttribute;
            if (attribute == null) return null;
            if (attribute.Group == 0) return null;
            return attribute.Group;
        }

        private int? GetRestGroup()
        {
            var group = Options.Groups.FirstOrDefault(g => g.Sequence.Any(t => t.TaskType == typeof(IStartupTask)));
            if (group == null) return null;
            return Options.Groups.IndexOf(group);            
        }
        
    }
}
