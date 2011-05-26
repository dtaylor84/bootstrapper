using System;

namespace Bootstrap.Extensions.StartupTasks
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TaskAttribute: Attribute
    {
        public int PositionInSequence { get; set; }
        public int DelayStartBy { get; set; }
        public int Group { get; set; }

        public TaskAttribute()
        {
            PositionInSequence = int.MaxValue;
            DelayStartBy = 0;
            Group = 0;
        }
    }
}
