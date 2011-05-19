using System;

namespace Bootstrap.Extensions.StartupTasks
{
    public class ExecutionLogEntry
    {
        public DateTime Timestamp { get; set; }
        public string TaskName { get; set; }
        public int SequencePosition { get; set; }
        public int DelayInMilliseconds { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }
    }
}
