namespace Bootstrap.Extensions.StartupTasks
{
    public interface ISequenceSpecial
    {
        ISequenceSpecification SequenceSpecification { get; }
        bool FirstInSequence { get; }
        ISequenceSpecification TheRest();
    }
}
