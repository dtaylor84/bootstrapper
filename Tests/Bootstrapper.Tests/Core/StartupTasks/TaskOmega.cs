using Bootstrap.StartupTasks;
using Bootstrap.Tests.Extensions.TestImplementations;

namespace Bootstrap.Tests.Core.StartupTasks
{
    [Task(PositionInSequence = 1)] public class TaskOmega : TestStartupTask { }
}