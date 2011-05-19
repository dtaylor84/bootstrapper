using System;
using System.Collections.Generic;

namespace Bootstrap.Extensions.StartupTasks
{
    public interface ISequenceSpecification
    {
        List<Type> Sequence { get; }
        ISequenceSpecification First<T>() where T : IStartupTask;
        ISequenceSpecial First();
        ISequenceSpecification Then<T>() where T : IStartupTask;
        ISequenceSpecial Then();
    }
}
