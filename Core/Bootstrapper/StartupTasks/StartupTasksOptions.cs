using System;
using System.Collections.Generic;
using Bootstrap.Extensions;

namespace Bootstrap.StartupTasks
{
    public class StartupTasksOptions: BootstrapperExtensionOptions
    {
        private readonly ISequenceSpecification taskSequence = new SequenceSpecification();
        public List<Type> Sequence { get { return taskSequence.Sequence; } }

        public StartupTasksOptions UsingThisExecutionOrder(Func<ISequenceSpecification, ISequenceSpecification> buildSequence)
        {
            buildSequence(taskSequence);
            return this;
        }

    }
}
