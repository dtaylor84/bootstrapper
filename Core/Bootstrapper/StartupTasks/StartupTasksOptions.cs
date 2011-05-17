using System;
using System.Collections.Generic;
using Bootstrap.Extensions;

namespace Bootstrap.StartupTasks
{
    public class StartupTasksOptions: BootstrapperExtensionOptions
    {
        public List<Type> Sequence { get; private set; }

        public StartupTasksOptions UsingSequence(ISequenceSpecification sequenceSpecification)
        {
            Sequence = sequenceSpecification.Sequence;
            return this;
        }

    }
}
