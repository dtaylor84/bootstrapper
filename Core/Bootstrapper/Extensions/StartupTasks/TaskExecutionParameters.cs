using System;

namespace Bootstrap.Extensions.StartupTasks
{
    public class TaskExecutionParameters
    {
        public IStartupTask Task { get; set; }
        public Type TaskType { get; set; }
        public int Position { get; set; }
        public int Delay { get; set; }
        public int Group { get; set; }
    }
}
