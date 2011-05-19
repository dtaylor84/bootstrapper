using System.Collections.Generic;

namespace Bootstrap.Extensions.StartupTasks
{
    public interface ISequenceSpecification
    {
        List<TaskExecutionParameters> Sequence { get; }
        ISequenceSpecification First<T>() where T : IStartupTask;
        ISequenceSpecial First();
        ISequenceSpecification Then<T>() where T : IStartupTask;
        ISequenceSpecial Then();
        ISequenceSpecification DelayStartBy(int milliseconds);
        ISequenceSpecification Seconds { get; }
        ISequenceSpecification MilliSeconds { get; }
    }
}
