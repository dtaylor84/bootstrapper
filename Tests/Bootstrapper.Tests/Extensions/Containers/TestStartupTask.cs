namespace Bootstrapper.Tests.Extensions.Containers
{
    public class TestStartupTask: IStartupTask
    {
        public static bool Invoked { get; set; }

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
