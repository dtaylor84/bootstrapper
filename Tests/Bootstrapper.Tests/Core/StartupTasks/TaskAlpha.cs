using Bootstrap.StartupTasks;
using Bootstrap.Tests.Extensions.TestImplementations;

namespace Bootstrap.Tests.Core.StartupTasks
{
    [Task(PositionInSequence = 2)] 
    public class TaskAlpha : TestStartupTask { }
}