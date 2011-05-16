namespace Bootstrap.StartupTasks
{
    public interface IStartupTask
    {
        void Run();
        void Reset();
    }
}
