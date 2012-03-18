using System.Collections.Generic;
using System.Threading;

namespace Bootstrap.Extensions.StartupTasks
{
    public class TaskGroup
    {
        public List<TaskExecutionParameters> Tasks { get; set; }
        public List<ExecutionLogEntry> ExecutionLog { get; set; }
        public Thread Thread { get; set; }

        public TaskGroup()
        {
            Tasks = new List<TaskExecutionParameters>();
            ExecutionLog = new List<ExecutionLogEntry>();
        }
    }
}
