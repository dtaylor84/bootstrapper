using Bootstrap.StartupTasks;

namespace Bootstrap.Tests.Extensions.TestImplementations
{
    public class TestStartupTask: IStartupTask
    {
        public static bool Invoked { get; private set; }

        public TestStartupTask()
        {
            Invoked = false;
        }

        public void Run()
        {
            Invoked = true;
        }
        public void Reset()
        {
            Invoked = false;
        }
    }
}
