using System.Collections.Generic;

namespace Bootstrap.Extensions.StartupTasks
{
    public class SequenceSpecification: ISequenceSpecification
    {
        private TaskExecutionParameters lastTask;

        public List<TaskExecutionParameters> Sequence {get; private set; }

        public SequenceSpecification()
        {
            lastTask = null;
            Sequence = new List<TaskExecutionParameters>();
        }

        public ISequenceSpecification First<T>() where  T:IStartupTask
        {
            lastTask = new TaskExecutionParameters {TaskType = typeof (T)};
            Sequence.Insert(0, lastTask);
            return this;
        }

        public ISequenceSpecial First()
        {
            return new SequenceSpecial(this, true);
        }

        public ISequenceSpecification Then<T>() where  T:IStartupTask
        {
            lastTask = new TaskExecutionParameters { TaskType = typeof(T) };
            Sequence.Add(lastTask);
            return this;
        }

        public ISequenceSpecial Then()
        {
            return new SequenceSpecial(this, false);
        }

        public ISequenceSpecification DelayStartBy(int milliseconds)
        {
            lastTask.Delay = milliseconds;
            return this;
        }

        public ISequenceSpecification Seconds
        {
            get { lastTask.Delay *= 1000; return this;}
        }

        public ISequenceSpecification MilliSeconds
        {
            get { return this; }
        }
    }
}
