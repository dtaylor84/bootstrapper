using Bootstrap.StartupTasks;

namespace Bootstrap.Tests.Extensions.TestImplementations
{
    public abstract class AbstractTestStartupTask: IStartupTask
    {
        public void Run() {}
        public void Reset() {}
    }
}