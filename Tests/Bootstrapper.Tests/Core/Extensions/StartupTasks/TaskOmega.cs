using Bootstrap.Extensions.StartupTasks;
using Bootstrap.Tests.Extensions.TestImplementations;

namespace Bootstrap.Tests.Core.Extensions.StartupTasks
{
    [Task(PositionInSequence = 1, DelayStartBy=30)] public class TaskOmega : TestStartupTask { }
}