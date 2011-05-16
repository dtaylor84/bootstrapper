namespace Bootstrap.StartupTasks
{
    public interface ISequenceSpecial
    {
        ISequenceSpecification SequenceSpecification { get; }
        bool FirstInSequence { get; }
        ISequenceSpecification TheRest();
    }
}
