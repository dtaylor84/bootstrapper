using System;
using System.Collections.Generic;

namespace Bootstrap.Extensions.StartupTasks
{
    public class StartupTasksOptions: BootstrapperExtensionOptions
    {
        private readonly ISequenceSpecification taskSequence = new SequenceSpecification();
        public List<ISequenceSpecification> Groups { get; set; }
        public List<TaskExecutionParameters> Sequence { get { return Groups[Groups.Count-1].Sequence; } }

        public StartupTasksOptions()
        {
            Groups = new List<ISequenceSpecification> {new SequenceSpecification()};
        }

        public StartupTasksOptions UsingThisExecutionOrder(Func<ISequenceSpecification, ISequenceSpecification> buildSequence)
        {
            buildSequence(Groups[Groups.Count-1]);
            return this;
        }

        public StartupTasksOptions WithGroup(Func<ISequenceSpecification, ISequenceSpecification> buildSequence)
        {
            if(Groups[0].Sequence.Count>0 ) Groups.Add(new SequenceSpecification());
            return UsingThisExecutionOrder(buildSequence);
        }

        public StartupTasksOptions AndGroup(Func<ISequenceSpecification, ISequenceSpecification> buildSequence)
        {
            return WithGroup(buildSequence);
        }
    }
}
