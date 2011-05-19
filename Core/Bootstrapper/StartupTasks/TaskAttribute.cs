using System;

namespace Bootstrap.StartupTasks
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TaskAttribute: Attribute
    {
        public int PositionInSequence { get; set; }
    }
}
