using System;
using System.Collections.Generic;

namespace Bootstrap.Extensions.StartupTasks
{
    public class StartupTasksOptions: BootstrapperExtensionOptions
    {
        private readonly ISequenceSpecification taskSequence = new SequenceSpecification();
        public List<TaskExecutionParameters> Sequence { get { return taskSequence.Sequence; } }

        public StartupTasksOptions UsingThisExecutionOrder(Func<ISequenceSpecification, ISequenceSpecification> buildSequence)
        {
            buildSequence(taskSequence);
            return this;
        }

    }
}
