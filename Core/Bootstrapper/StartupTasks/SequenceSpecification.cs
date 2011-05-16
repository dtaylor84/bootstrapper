using System;
using System.Collections.Generic;

namespace Bootstrap.StartupTasks
{
    public class SequenceSpecification: ISequenceSpecification
    {
        public List<Type> Sequence {get; private set; }

        public SequenceSpecification()
        {
            Sequence = new List<Type>();
        }

        public ISequenceSpecification First<T>() where  T:IStartupTask
        {
            Sequence.Insert(0, typeof(T));
            return this;
        }

        public ISequenceSpecial First()
        {
            return new SequenceSpecial(this, true);
        }

        public ISequenceSpecification Then<T>() where  T:IStartupTask
        {
            Sequence.Add(typeof(T));
            return this;
        }

        public ISequenceSpecial Then()
        {
            return new SequenceSpecial(this, false);
        }
    }
}
