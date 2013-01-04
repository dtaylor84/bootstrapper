namespace Bootstrap.Extensions.StartupTasks
{
    public interface IStartupTask
    {
        void Run();
        void Reset();
    }
}
