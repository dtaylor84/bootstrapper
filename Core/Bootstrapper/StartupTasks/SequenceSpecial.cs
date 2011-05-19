namespace Bootstrap.StartupTasks
{
    public class SequenceSpecial: ISequenceSpecial
    {
        public ISequenceSpecification SequenceSpecification { get; private set; }
        public bool FirstInSequence { get; private set; }

        public SequenceSpecial(ISequenceSpecification sequenceSpecification, bool firstInSequence)
        {
            SequenceSpecification = sequenceSpecification;
            FirstInSequence = firstInSequence;
        }

        public ISequenceSpecification TheRest()
        {
            if (FirstInSequence) SequenceSpecification.First<IStartupTask>(); 
            else SequenceSpecification.Then<IStartupTask>();
            return SequenceSpecification;
        }
    }
}
