namespace Bootstrap.Extensions.StartupTasks
{
    public class TaskExecutionParameters
    {
        public IStartupTask Task { get; set; }
        public int Position { get; set; }
        public int Delay { get; set; }
    }
}
